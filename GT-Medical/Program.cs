using GT_Medical.UI;

namespace GT_Medical
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
            Application.ThreadException += (s, e) =>
            {
                // log the exception, display it, etc
                MessageBox.Show(e.Exception.Message + "\n" + e.Exception.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            Application.Run(new FrmVideoPlayer());
            
        }
    }
}