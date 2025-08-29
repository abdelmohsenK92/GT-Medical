using GT_Medical.UI;
using Microsoft.VisualBasic.Devices;
using System;
using System.Configuration;

namespace GT_Medical
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            try
            {
                ApplicationConfiguration.Initialize();
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (s, e) =>
                {
                    // log the exception, display it, etc
                    MessageBox.Show(e.Exception.Message + "\n" + e.Exception.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
                Startup.RunApp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}