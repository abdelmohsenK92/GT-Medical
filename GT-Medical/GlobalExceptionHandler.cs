using GT_Medical.Abstractions;
using GT_Medical.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace GT_Medical
{
    public sealed class GlobalExceptionHandler : ISingletonService
    {
        private readonly ILogger<GlobalExceptionHandler> _log;
        private readonly IHostEnvironment _env;
        private readonly ExceptionHandlingOptions _opts;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> log,
            IHostEnvironment env,
            IOptions<ExceptionHandlingOptions> opts)
        {
            _log = log;
            _env = env;
            _opts = opts.Value;
        }

        public void Register()
        {
            // Catch UI thread exceptions
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += OnThreadException;

            // Catch non-UI / background thread exceptions
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        private void OnThreadException(object? sender, ThreadExceptionEventArgs e)
            => Handle(e.Exception, "Application.ThreadException", isTerminating: false);

        private void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
            => Handle(e.ExceptionObject as Exception ?? new Exception("Unknown unhandled exception"),
                      "AppDomain.CurrentDomain.UnhandledException",
                      isTerminating: e.IsTerminating);

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            Handle(e.Exception, "TaskScheduler.UnobservedTaskException", isTerminating: false);
            e.SetObserved();
        }

        private void Handle(Exception ex, string source, bool isTerminating)
        {
            try
            {
                var details = BuildReport(ex, source, isTerminating);
                _log.LogError(ex, "Global exception from {Source}. Terminating={Terminating}", source, isTerminating);
                WriteCrashFile(details);

                if (_opts.ShowUiDialog)
                    ShowDialog(ex, details, isTerminating);
            }
            catch
            {
                // Last-chance: never throw from the handler
            }
            finally
            {
                if (isTerminating && _opts.ExitOnFatal)
                {
                    try { _log.LogCritical("Process will exit due to fatal exception."); } catch { }
                    Environment.Exit(-1);
                }
            }
        }

        private string BuildReport(Exception ex, string source, bool isTerminating)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Crash Report ===");
            sb.AppendLine($"UTC Time   : {DateTime.UtcNow:O}");
            sb.AppendLine($"Environment: {_env.EnvironmentName}");
            sb.AppendLine($"Source     : {source}");
            sb.AppendLine($"Terminating: {isTerminating}");
            sb.AppendLine($"OS         : {Environment.OSVersion}");
            sb.AppendLine($"Process    : {Environment.ProcessPath}");
            sb.AppendLine();
            sb.AppendLine(ex.ToString());
            return sb.ToString();
        }

        private void WriteCrashFile(string details)
        {
            try
            {
                var dir = Path.GetFullPath(_opts.CrashDir);
                Directory.CreateDirectory(dir);
                var file = Path.Combine(dir, $"crash_{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}.log");
                File.WriteAllText(file, details, Encoding.UTF8);
            }
            catch { /* ignore IO errors */ }
        }

        private void ShowDialog(Exception ex, string details, bool isTerminating)
        {
            try
            {
                var message = _env.IsDevelopment()
                    ? $"An error occurred:\n\n{ex.Message}\n\n{ex.StackTrace}"
                    : "Sorry, something went wrong. A report has been saved.";

                var caption = isTerminating ? "Fatal Error" : "Unexpected Error";

                var result = MessageBox.Show(
                    message + "\n\nCopy error details to clipboard?",
                    caption,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                    Clipboard.SetText(details);
            }
            catch
            {
                // UI is best-effort
            }
        }
    }
}