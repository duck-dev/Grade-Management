using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using GradeManagement.Enums;
using GradeManagement.ViewModels;
using GradeManagement.Views;

namespace GradeManagement
{
    public class App : Application
    {
        private static readonly string _resourcesPath 
            = Path.Combine(("avares:" + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar + "GradeManagement"), "src", "Resources");
        
        private ThemeMode _theme;
        private Uri? _themeSource;
        private StyleInclude? _currentThemeStyle;
        private readonly Dictionary<ThemeMode, Uri> _themeSourcesCollection = new()
        {
            { ThemeMode.Light, new Uri(Path.Combine(_resourcesPath, "LightTheme.axaml")) },
            { ThemeMode.Dark, new Uri(Path.Combine(_resourcesPath, "DarkTheme.axaml")) }
        };

        public App() => SetTheme(ThemeMode.Light); // TODO: Get from saved settings

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel() { AppInstance = this }
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        internal void SetTheme(ThemeMode theme)
        {
            _theme = theme;
            _themeSource = _themeSourcesCollection[theme];

            if (_currentThemeStyle is not null)
                this.Styles.Remove(_currentThemeStyle);
            _currentThemeStyle = new StyleInclude(_themeSourcesCollection[theme])
            {
                Source = _themeSourcesCollection[theme]
            };
            this.Styles.Add(_currentThemeStyle);
        }
    }
}