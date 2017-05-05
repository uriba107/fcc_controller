using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fcc3_configurator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///     
        static Mutex mutex = new Mutex(true, "{2B1592A2-23E4-493D-90C2-1B6DDADCB221}");

        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {

                Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("only one instance at a time");
            }
        }
    }
}
