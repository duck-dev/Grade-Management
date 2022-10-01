using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.UtilityCollection;

namespace GradeManagement.ResourcesNamespace
{
    public static partial class Resources
    {
        private const int StyleIndex = 2;
        
        private static readonly LightTheme _lightThemeInstance = new();
        private static readonly DarkTheme _darkThemeInstance = new();
        private static readonly Dictionary<ThemeMode, Theme> _themesLookup = new()
        {
            { ThemeMode.Light, _lightThemeInstance }, { ThemeMode.Dark, _darkThemeInstance }
        };

        public static Theme CurrentTheme { get; private set; } = _lightThemeInstance;

        // SolidColorBrush

        public static readonly SolidColorBrush FullyTransparentBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "FullyTransparent", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#0000FFFF"));
        public static readonly SolidColorBrush AppGreenBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "AppGreen", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#009B72"));
        public static readonly SolidColorBrush VariantAppGreenBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "VariantAppGreen", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#00AD7F"));
        public static readonly SolidColorBrush LightPurpleBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "LightPurple", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#A5B1CC"));
        public static readonly SolidColorBrush InvalidColorBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "InvalidColor", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#D64045"));
        
        public static readonly SolidColorBrush LightGreenContextMenuBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "LightGreenContextMenu", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#58C4A8"));
        public static readonly SolidColorBrush DarkerLightGreenContextMenuBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "DarkerLightGreenContextMenu", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#0EB085"));
        public static readonly SolidColorBrush LightRedContextMenuBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "LightRedContextMenu", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#D4735B"));
        public static readonly SolidColorBrush DarkerLightRedContextMenuBrush =
            Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "DarkerLightRedContextMenu", StyleIndex) 
            ?? new SolidColorBrush(Color.Parse("#C96047"));
        
        // Colors
        public static readonly Color FullyTransparent = FullyTransparentBrush.Color;
        public static readonly Color AppGreen = AppGreenBrush.Color;
        public static readonly Color VariantAppGreen = VariantAppGreenBrush.Color;
        public static readonly Color LightPurple = LightPurpleBrush.Color;
        public static readonly Color InvalidColor = InvalidColorBrush.Color;
        
        public static readonly Color LightGreenContextMenu = LightGreenContextMenuBrush.Color;
        public static readonly Color DarkerLightGreenContextMenu = DarkerLightGreenContextMenuBrush.Color;
        public static readonly Color LightRedContextMenu = LightRedContextMenuBrush.Color;
        public static readonly Color DarkerLightRedContextMenu = DarkerLightRedContextMenuBrush.Color;

        internal static void SetTheme(ThemeMode themeMode)
        {
            if (_themesLookup.ContainsKey(themeMode))
                CurrentTheme = _themesLookup[themeMode];
        }
    }
}