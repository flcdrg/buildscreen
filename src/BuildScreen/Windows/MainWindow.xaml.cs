using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using BuildScreen.ContinousIntegration;
using BuildScreen.ContinousIntegration.Exceptions;
using BuildScreen.ContinousIntegration.Persistance;
using BuildScreen.Core.Security;
using BuildScreen.Plugin;
using BuildScreen.Properties;
using BuildScreen.Resources;

namespace BuildScreen
{
    public partial class MainWindow
    {
        readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private PluginHandler _pluginHandler;
        private IClientFactory _clientFactory;

        public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel();

        #region Lifetime

        public MainWindow()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShortcutsInfo.Text = InternalResources.ShortcutInfoEnterFullScreen;

            _clientFactory = new ClientFactory();
            _pluginHandler = new PluginHandler();

            UpdateBuilds();

            _dispatcherTimer.Tick += UpdateTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, Settings.Default.RefreshInterval);
            _dispatcherTimer.Start();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F11:
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    Topmost = true;

                    ShortcutsInfo.Text = InternalResources.ShortcutInfoExitFullScreen;
                    break;
                case Key.Escape:
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Normal;
                    Topmost = false;

                    ShortcutsInfo.Text = InternalResources.ShortcutInfoEnterFullScreen;
                    break;
                default:
                    if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O)
                    {
                        _dispatcherTimer.Stop();

                        var optionsDialog = new OptionsWindow();
                        optionsDialog.Closing += OptionsWindow_Closing;
                        optionsDialog.ShowDialog();
                    }
                    break;
            }
        }

        private void OptionsWindow_Closing(object sender, CancelEventArgs e)
        {
            UpdateBuilds();

            _dispatcherTimer.Interval = new TimeSpan(0, 0, Settings.Default.RefreshInterval);
            _dispatcherTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateBuilds();
        }

        #endregion

        #region Configuration

        private static ClientConfiguration LoadClientConfiguration()
        {
            var baseUrl = string.Empty;

            switch (Settings.Default.ClientType)
            {
                case ClientType.TeamCity:
                    baseUrl = Settings.Default.BaseUrlTeamCity;
                    break;
                case ClientType.Hudson:
                    baseUrl = Settings.Default.BaseUrlHudson;
                    break;
                case ClientType.Bamboo:
                    baseUrl = Settings.Default.BaseUrlBamboo;
                    break;
            }

            return new ClientConfiguration
            {
                Domain = Settings.Default.Domain,
                Port = Settings.Default.Port,
                UserName = Settings.Default.UserName,
                Password = SecureData.Decrypt(Settings.Default.Password),
                UseSsl = Settings.Default.UseSsl,
                IgnoreInvalidCertificate = Settings.Default.IgnoreInvalidCertificate,
                ClientType = Settings.Default.ClientType,
                BaseUrl = baseUrl
            };
        }

        #endregion

        #region Updates

        private void UpdateBuilds()
        {
            ViewModel.Builds.Clear();

            var clientConfiguration = LoadClientConfiguration();
            if (!clientConfiguration.AllDataExists) return;

            try
            {
                using (var client = _clientFactory.CreateClient(clientConfiguration))
                {
                    if (!client.IsConnected) return;

                    foreach (var storedBuild in Settings.Default.Builds)
                    {
                        try
                        {
                            var build = client.BuildByUniqueIdentifier(storedBuild.UniqueIdentifier);

                            _pluginHandler.TriggerPlugins(storedBuild, build);

                            var daysLastBuildHappened = DateTime.Now.Subtract(build.StartDate).Days;

                            if (!Settings.Default.HideInactive ||
                                Settings.Default.HideInactive && daysLastBuildHappened < Settings.Default.HideInactiveWeeks * 7)
                            {
                                ViewModel.Builds.Add(new BuildViewModel(build));
                            }
                        }
                        catch (ClientLoadDocumentException)
                        {
                            // TODO: One build could not get loaded via typeId, maybe it got deleted but is still in the settings; Display error message
                        }
                    }
                }
            }
            catch (ClientConnectionException)
            {
                // TODO: Display error message
            }
        }

        #endregion
    }
}
