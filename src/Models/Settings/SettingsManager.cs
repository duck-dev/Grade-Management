using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            {
                SaveSettings();
                return;
            }

            string content = File.ReadAllText(FilePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            Settings = JsonSerializer.Deserialize<Preferences>(content, options);
        }

        internal static void SaveSettings()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            string jsonString = JsonSerializer.Serialize(Settings, options);
            File.WriteAllText(FilePath, jsonString);
        }
    }
}