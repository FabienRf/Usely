# Usely

A lightweight Windows utility for configurable hotkeys, window management, and
quick automation.

## 🛠️ Technologies

- .NET 10 (net10.0-windows)
- C#
- WPF (XAML)
- MVVM pattern
- MSBuild / Visual Studio
- PowerShell (setup script)
- NuGet

## 📋 Features

- Global hotkeys with a user-facing hotkey editor
- Configurable application settings and persistence
- Mouse click automation and utilities
- Window management helpers for focused workflows
- In-app drawing toolbar for quick annotations
- Hotkey update dialog and UI views for configuration

## 💡 How it can be improved

- Add cross-platform support (e.g., port to Avalonia)
- Add unit and integration tests with CI (GitHub Actions)
- Provide an installer and automatic updates
- Add cloud-sync for user settings and profiles
- Improve accessibility and localization support
- Add telemetry (opt-in) and usage analytics

## ⚡ Running the project

### Prerequisites

- Windows 10/11
- .NET 10 SDK installed
- Visual Studio 2022 or later (recommended) or the `dotnet` CLI

### Clone and run (CLI)

```powershell
# from repository root
cd Usely
dotnet restore
dotnet build -c Debug
dotnet run --project Usely.csproj -c Debug
```

### Run in Visual Studio

- Open the `Usely` folder or `Usely.csproj` in Visual Studio and start the
  debugger (F5).

## 🍿 Video

Demo video: _Insert demo video link here_
