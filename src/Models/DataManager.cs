using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GradeManagement.Models
{
    public static class DataManager
    {
        internal static List<SchoolYear>? SchoolYears { get; set; } = new();
        
        private static string? FilePath 
            => Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/SchoolYears.json";

        internal static void LoadData()
        {
            if (FilePath is null || !File.Exists(FilePath))
                return;

            string content = File.ReadAllText(FilePath);
            SchoolYears = JsonSerializer.Deserialize<List<SchoolYear>>(content);
        }

        internal static void SaveData(IEnumerable<SchoolYear> year)
        {
            if (FilePath is null)
                return;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(year, options);
            File.WriteAllText(FilePath, jsonString);
        }
    }
}