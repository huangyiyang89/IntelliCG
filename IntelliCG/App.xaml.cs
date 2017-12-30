using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IntelliCG
{
    
    public partial class App : Application
    {
        private static readonly Mutex Mutex = new Mutex(false, "IntelliCG");
        protected override void OnStartup(StartupEventArgs e)
        {

#if DEBUG
            base.OnStartup(e);
            return;

#else


            // Wait 5 seconds if contended – in case another instance
            // of the program is in the process of shutting down.
            if (!Mutex.WaitOne(TimeSpan.FromSeconds(5), false))
            {
                Application.Current.Shutdown();
                return;
            }
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            var runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            if (!runAsAdmin)
            {
                // It is not possible to launch a ClickOnce app as administrator directly,
                // so instead we launch the app as administrator in a new process.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase)
                {
                    // The following properties run the new process as administrator
                    UseShellExecute = true,
                    Verb = "runas"
                };
                // Start the new process
                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception)
                {
                    // The user did not allow the application to run as administrator
                    MessageBox.Show("请以管理员身份重新启动程序!");
                }

                // Shut down the current process
                Environment.Exit(0);
            }
            else
            {
                base.OnStartup(e);
            }
#endif


        }
    }
}
