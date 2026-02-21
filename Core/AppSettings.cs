using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Usely.ViewModels;

namespace Usely.Core
{
    // Loads hotkey settings from appsettings.json and exposes them for binding
    public class AppSettingsHotkeys
    {
        // Collection used for binding in the UI
        public ObservableCollection<AppSettingsView.HotkeyView> Hotkeys { get; }

        public AppSettingsHotkeys()
        {
            Hotkeys = new ObservableCollection<AppSettingsView.HotkeyView>();

            try
            {
                // Resolve appsettings.json next to the executable
                var baseDir = AppContext.BaseDirectory;
                var settingsPath = Path.Combine(baseDir, "appsettings.json");

                if (!File.Exists(settingsPath))
                    return; // nothing to load, keep collection empty

                string json = File.ReadAllText(settingsPath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var config = JsonSerializer.Deserialize<Root>(json, options);
                if (config?.Hotkeys == null)
                    return;

                foreach (var item in config.Hotkeys)
                {
                    var hotkey = item.Value;
                    Hotkeys.Add(new AppSettingsView.HotkeyView(
                        actionName: item.Key,
                        keyLib: hotkey.KeyLib,
                        windowActionKey: hotkey.WindowActionKey));
                }
            }
            catch
            {
                // In production you might log this; here we fail gracefully
            }
        }
    }

    // Matches the shape of appsettings.json
    public class Root
    {
        public Dictionary<string, Hotkey> Hotkeys { get; set; } = new();
    }

    public class Hotkey
    {
        public string KeyLib { get; set; } = string.Empty;
        public string WindowActionKey { get; set; } = string.Empty;
    }
}
