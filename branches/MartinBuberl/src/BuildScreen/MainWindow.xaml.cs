using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using BuildScreen.Core.Extensions;
using BuildScreen.Resources;

namespace BuildScreen
{
    public partial class MainWindow : Window
    {
        /// <summary>Timer to measure user activity and inactivity.</summary>
        private readonly DispatcherTimer _activityTimer;
        /// <summary>Mouse position when the application became inactive.</summary>
        private Point _inactiveMousePosition = new Point(0, 0);
        /// <summary>Identify if the window state is maximized.</summary>
        private bool _isWindowStateMaximized;

        public MainWindow()
        {
            InitializeComponent();

            // handle user activity and inactivity
            InputManager.Current.PreProcessInput += OnActivity;
            _activityTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(5), IsEnabled = true };
            _activityTimer.Tick += OnInactivity;

            // handle screensaver and display power saving
            SourceInitialized += OnSourceInitialized;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ShortcutsInfo.Text = PresentationResources.MainWindow_ShortcutInfoEnterFullScreen;
        }

        private void OnInactivity(object sender, EventArgs e)
        {
            _inactiveMousePosition = Mouse.GetPosition(MainGrid);
            ShortcutsInfo.Visibility = Visibility.Hidden;
        }

        private void OnActivity(object sender, PreProcessInputEventArgs e)
        {
            InputEventArgs inputEventArgs = e.StagingItem.Input;

            if (inputEventArgs is MouseEventArgs || inputEventArgs is KeyboardEventArgs)
            {
                if (e.StagingItem.Input is MouseEventArgs)
                {
                    MouseEventArgs mouseEventArgs = (MouseEventArgs)e.StagingItem.Input;

                    // no button is pressed and the position is still the same as the application became inactive
                    if (mouseEventArgs.NoButtonIsPressed() && _inactiveMousePosition == mouseEventArgs.GetPosition(MainGrid))
                        return;
                }

                ShortcutsInfo.Visibility = Visibility.Visible;

                _activityTimer.Stop();
                _activityTimer.Start();
            }
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = (HwndSource)PresentationSource.FromVisual(this);

            if (source != null)
                source.AddHook(Hook);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11 && !_isWindowStateMaximized)
            {
                _isWindowStateMaximized = true;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                Topmost = true;

                RenderWindowState();
            }

            if (e.Key == Key.Escape && _isWindowStateMaximized)
            {
                _isWindowStateMaximized = false;
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResizeWithGrip;
                Topmost = false;

                RenderWindowState();
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O)
            {
                OptionsWindow optionsDialog = new OptionsWindow();
                optionsDialog.Closing += OnOptionsWindowClosing;
                optionsDialog.ShowDialog();
            }
        }

        private void OnOptionsWindowClosing(object sender, CancelEventArgs e)
        {
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Maximized && !_isWindowStateMaximized)
            {
                _isWindowStateMaximized = true;
                // WPF seems to make the decision about whether to go full-screen or respect the taskbar based on the window style at the time of 
                // maximization. The solution is to switch the window state back to normal, set the window style, and then set the window back to 
                // maximized again. http://stackoverflow.com/questions/4939219/wpf-full-sreen-on-maximize
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                Topmost = true;

                RenderWindowState();
            }

            base.OnStateChanged(e);
        }

        private void RenderWindowState()
        {
            if (_isWindowStateMaximized)
            {
                ShortcutsInfo.Text = PresentationResources.MainWindow_ShortcutInfoExitFullScreen;
                ShortcutsInfo.Padding = new Thickness(0, 23, 27, 0);
            }
            else
            {
                ShortcutsInfo.Text = PresentationResources.MainWindow_ShortcutInfoEnterFullScreen;
                ShortcutsInfo.Padding = new Thickness(0, 5, 9, 0);
            }
        }

        private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // http://www.pixvillage.com/blogs/devblog/archive/2007/02/27/6493.aspx
            const int WM_SYSCOMMAND = 0x112;
            const int SC_SCREENSAVE = 0xF140;
            const int SC_MONITORPOWER = 0xF170;

            // only handle screensaver and display power saving if the application is maximized
            if (_isWindowStateMaximized && msg == WM_SYSCOMMAND && (((long)wParam & 0xFFF0) == SC_SCREENSAVE || ((long)wParam & 0xFFF0) == SC_MONITORPOWER))
                handled = true;

            return IntPtr.Zero;
        }
    }
}
