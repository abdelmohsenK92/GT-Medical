namespace GT_Medical.Models
{
    public sealed class ExceptionHandlingOptions
    {
        public const string SectionName = "ExceptionHandling";
        public bool ShowUiDialog { get; set; } = true;
        public string CrashDir { get; set; } = "crashlogs";
        public bool ExitOnFatal { get; set; } = true;
    }
}
