using System;
using System.Windows;

namespace BuildScreen
{
    /// <summary>
    /// The startup object defined in the projects properties.
    /// </summary>
    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SingleInstanceManager singleInstanceManager = new SingleInstanceManager();
            singleInstanceManager.Run(args);
        }
    }

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        public void Activate()
        {
            MainWindow.Activate();
        }
    }

    /// <summary>
    /// Using Visual Basic bits to detect single instances and process accordingly:
    ///  - OnStartup is fired when the first instance loads
    ///  - OnStartupNextInstance is fired when the application is re-run again
    /// </summary>
    /// <remarks>
    /// It is redirected to this instance thanks to IsSingleInstance.
    /// </remarks>
    /// <see href="http://pietschsoft.com/post/2009/01/Single-Instance-WPF-Application-in-NET-3.aspx"/>
    public class SingleInstanceManager : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        App app;

        public SingleInstanceManager()
        {
            IsSingleInstance = true;
        }

        /// <summary>
        /// First time the application is launched.
        /// </summary>
        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
        {
            app = new App();
            app.Run();

            return false;
        }

        /// <summary>
        /// Subsequent launches of the same application.
        /// </summary>
        /// <param name="eventArgs"></param>
        protected override void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            app.Activate();
        }
    }
}
