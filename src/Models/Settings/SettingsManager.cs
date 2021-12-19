using System;

namespace GradeManagement.Models.Settings
{
    public static class SettingsManager
    {
        internal static event EventHandler? LoadingSettings; // Subscribe in static-constructor of a `Settings` class

        internal static void LoadSettings()
        {
            LoadingSettings?.Invoke(null, EventArgs.Empty);
            LoadingSettings = null;
        }
    }
}