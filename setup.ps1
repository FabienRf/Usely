# setup.ps1
# Script pour créer la structure complète d'un projet WPF minimal

$ProjectName = "Usely"

# 1. Créer projet WPF .NET
dotnet new wpf -n $ProjectName

Set-Location $ProjectName

# 2. Créer dossiers
$folders = @("Core", "Models", "ViewModels", "Views", "Helpers", "Resources", "Resources\Icons")
foreach ($f in $folders) {
    New-Item -ItemType Directory -Path $f | Out-Null
}

# 3. Créer fichiers avec contenu de base

# App.xaml
@"
<Application x:Class="$ProjectName.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary Source="Resources/Styles.xaml"/>
    </Application.Resources>
</Application>
"@ | Out-File -Encoding utf8 "App.xaml"

# App.xaml.cs
@"
using System.Windows;

namespace $ProjectName
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
"@ | Out-File -Encoding utf8 "App.xaml.cs"

# MainWindow.xaml
@"
<Window x:Class="$ProjectName.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="$ProjectName" Height="350" Width="525">
    <Grid>
        <StackPanel Margin="20">
            <Button Content="Put It On Top" Height="40" Margin="5"/>
            <Button Content="Capture Screen" Height="40" Margin="5"/>
            <Button Content="Drawing Mode" Height="40" Margin="5"/>
        </StackPanel>
    </Grid>
</Window>
"@ | Out-File -Encoding utf8 "MainWindow.xaml"

# MainWindow.xaml.cs
@"
using System.Windows;

namespace $ProjectName
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
"@ | Out-File -Encoding utf8 "MainWindow.xaml.cs"

# appsettings.json
@"
{
  "Hotkeys": {
    "PutItOnTop": "Ctrl+Shift+A",
    "Capture": "Ctrl+Shift+S",
    "DrawMode": "Ctrl+Shift+D"
  }
}
"@ | Out-File -Encoding utf8 "appsettings.json"

# Resources/Styles.xaml
@"
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Style TargetType="Button">
        <Setter Property="FontSize" Value="16"/>
        <Setter Property="Margin" Value="5"/>
    </Style>
</ResourceDictionary>
"@ | Out-File -Encoding utf8 "Resources/Styles.xaml"

# Core/HotkeyManager.cs
@"
namespace $ProjectName.Core
{
    public class HotkeyManager
    {
        public void RegisterHotkeys()
        {
        }
    }
}
"@ | Out-File -Encoding utf8 "Core/HotkeyManager.cs"

# Core/WindowManager.cs
@"
namespace $ProjectName.Core
{
    public class WindowManager
    {
        public void SetPutItOnTop(System.IntPtr windowHandle, bool top)
        {
        }
    }
}
"@ | Out-File -Encoding utf8 "Core/WindowManager.cs"

# Core/ScreenCaptureService.cs
@"
namespace $ProjectName.Core
{
    public class ScreenCaptureService
    {
    }
}
"@ | Out-File -Encoding utf8 "Core/ScreenCaptureService.cs"

# Core/OcrService.cs
@"
namespace $ProjectName.Core
{
    public class OcrService
    {
    }
}
"@ | Out-File -Encoding utf8 "Core/OcrService.cs"

# Core/DrawingOverlayService.cs
@"
namespace $ProjectName.Core
{
    public class DrawingOverlayService
    {
    }
}
"@ | Out-File -Encoding utf8 "Core/DrawingOverlayService.cs"

# Models/AppSettings.cs
@"
namespace $ProjectName.Models
{
    public class AppSettings
    {
        public Hotkeys Hotkeys { get; set; }
    }

    public class Hotkeys
    {
        public string PutItOnTop { get; set; }
        public string Capture { get; set; }
        public string DrawMode { get; set; }
    }
}
"@ | Out-File -Encoding utf8 "Models/AppSettings.cs"

# Models/CaptureResult.cs
@"
namespace $ProjectName.Models
{
    public class CaptureResult
    {
    }
}
"@ | Out-File -Encoding utf8 "Models/CaptureResult.cs"

# ViewModels/MainViewModel.cs
@"
namespace $ProjectName.ViewModels
{
    public class MainViewModel
    {
    }
}
"@ | Out-File -Encoding utf8 "ViewModels/MainViewModel.cs"

# Views/OverlayWindow.xaml
@"
<Window x:Class="$ProjectName.Views.OverlayWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Overlay" Height="300" Width="400" WindowStyle="None" AllowsTransparency="True" Background="Transparent">
    <Grid>
    </Grid>
</Window>
"@ | Out-File -Encoding utf8 "Views/OverlayWindow.xaml"

# Views/OverlayWindow.xaml.cs
@"
using System.Windows;

namespace $ProjectName.Views
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();
        }
    }
}
"@ | Out-File -Encoding utf8 "Views/OverlayWindow.xaml.cs"

# Views/DrawingToolbar.xaml
@"
<Window x:Class="$ProjectName.Views.DrawingToolbar"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Toolbar" Height="100" Width="300">
    <StackPanel Orientation="Horizontal">
    </StackPanel>
</Window>
"@ | Out-File -Encoding utf8 "Views/DrawingToolbar.xaml"

# Views/DrawingToolbar.xaml.cs
@"
using System.Windows;

namespace $ProjectName.Views
{
    public partial class DrawingToolbar : Window
    {
        public DrawingToolbar()
        {
            InitializeComponent();
        }
    }
}
"@ | Out-File -Encoding utf8 "Views/DrawingToolbar.xaml.cs"

# Helpers/NativeMethods.cs
@"
namespace $ProjectName.Helpers
{
    public class NativeMethods
    {
    }
}
"@ | Out-File -Encoding utf8 "Helpers/NativeMethods.cs"

# Helpers/ImageHelper.cs
@"
namespace $ProjectName.Helpers
{
    public class ImageHelper
    {
    }
}
"@ | Out-File -Encoding utf8 "Helpers/ImageHelper.cs"

Write-Host "`n✅ Projet $ProjectName créé avec tous les fichiers et dossiers de base."