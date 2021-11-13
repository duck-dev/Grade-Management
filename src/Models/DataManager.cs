using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public static class DataManager
    {
        internal static SchoolYear[]? SchoolYears { get; set; } = Array.Empty<SchoolYear>();

        internal static void LoadData()
        {
            
        }

        internal static void SaveData(SchoolYear year)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(year, options);
            Utilities.Log(jsonString);
        }
    }
}