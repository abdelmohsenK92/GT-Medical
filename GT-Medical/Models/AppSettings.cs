using GT_Medical.Abstractions;
using Newtonsoft.Json;

namespace GT_Medical.Models
{
    public class AppSettings : ISingletonService
    {
        public string LocalVideosUrl { get; set; }
        private static AppSettings _current;
        public static AppSettings Current
        {
            get
            {
                return _current ?? GetSettings();
            }
            set
            {
                SetSettings(value);
            }
        }
        public static void SetSettings(AppSettings settings)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var settingPath = baseDirectory + "\\AppSettings.dll";
            var json = JsonConvert.SerializeObject(settings,
                Formatting.Indented);
            var tmp = settingPath + ".tmp";

            File.WriteAllText(tmp, json);

            if (File.Exists(settingPath))
                File.Replace(tmp, settingPath, null);
            else
                File.Move(tmp, settingPath);
            _current = settings;
        }
        static AppSettings GetSettings()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var settingPath = baseDirectory + "\\AppSettings.dll";
            if (File.Exists(settingPath))
            {
                var json = File.ReadAllText(settingPath);
                _current = JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings()
                {
                    LocalVideosUrl = baseDirectory + "\\Videos"
                };
            }
            else
                _current = new AppSettings()
                {
                    LocalVideosUrl = baseDirectory + "\\Videos"
                };
            return _current;
        }
    }
}
