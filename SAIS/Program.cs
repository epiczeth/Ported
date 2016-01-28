using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace SAIS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            new Config().GetConfig();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSplash());

        }
    }
}
