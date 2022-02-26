using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GradeManagement.Enums;
using GradeManagement.ViewModels;
using GradeManagement.Views;

namespace GradeManagement
{
    public class App : Application
    {
        private const string ResourcesPath = "/src/Resources/";

        private ThemeMode _theme;
        private readonly Dictionary<ThemeMode, Uri> _themeSourcesCollection = new()
        {
            { ThemeMode.Light, new Uri(Path.Combine(ResourcesPath, "LightTheme.axaml")) },
            { ThemeMode.Dark, new Uri(Path.Combine(ResourcesPath, "DarkTheme.axaml")) },
        };

        private Uri _themeSource;
        private readonly StyledProperty<Uri> _themeSourceProperty = AvaloniaProperty.Register<App, Uri>(nameof(ThemeSource));

        public App() => _themeSource = ThemeSource = _themeSourcesCollection[ThemeMode.Light]; // TODO: Get from saved settings

        internal ThemeMode Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                ThemeSource = _themeSourcesCollection[_theme];
            }
        }

        private Uri ThemeSource
        {
            get => _themeSource; 
            set => this.SetAndRaise(_themeSourceProperty, ref _themeSource, value);
        }

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
    }
}