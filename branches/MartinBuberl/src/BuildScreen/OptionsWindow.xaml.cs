using System.Windows;
using BuildScreen.Data;
using BuildScreen.Properties;

namespace BuildScreen
{
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();

            this.BindInXaml();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void BindInXaml()
        {
            base.DataContext = new Connection
            {
                Domain = Settings.Default.OptionsSetup_Server_Domain,
                Port = Settings.Default.OptionsSetup_Server_Port.ToString(),
                SecureSocketLayer = Settings.Default.OptionsSetup_Server_SecureSocketLayer,
                UserName = Settings.Default.OptionsSetup_UserCredentials_UserName,
                Password = Settings.Default.OptionsSetup_UserCredentials_Password
            };
        }

        private void ButtonOkay_OnClick(object sender, RoutedEventArgs e)
        {
            var bla = DataContext;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
