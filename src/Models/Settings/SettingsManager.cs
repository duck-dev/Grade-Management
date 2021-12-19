using System.IO;
using System.Text.Json;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models.Settings
{
    public static class SettingsManager
    {
        internal static Preferences? Settings { get; private set; } = new();
        private static string FilePath => Utilities.FilesParentPath + "/Settings.json";

        internal static void LoadSettings()
        {
            if (!File.Exists(FilePath))
                return;

            string content = File.ReadAllText(FilePath);
            Settings = JsonSerializer.Deserialize<Preferences>(content);
        }

        internal static void SaveSettings()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Settings, options);
            File.WriteAllText(FilePath, jsonString);
        }
    }
}