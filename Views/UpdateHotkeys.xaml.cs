using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Usely.Core;

namespace Usely.Views
{
    public partial class UpdateHotkeys : Window
    {
        private readonly string _hotkeysName;
        private readonly HotkeyManager? _hotkeyManager;
        private bool _isCaptured = false;

        public UpdateHotkeys(string actionName, HotkeyManager? hotkeyManager)
        {
            InitializeComponent();
            _hotkeysName = actionName;
            _hotkeyManager = hotkeyManager;
            this.KeyDown += RebindHotkeys;
        }

        private void RebindHotkeys(object sender, KeyEventArgs e)
        {
            if (_isCaptured) return;

            _isCaptured = true;
            string keyString = BuildKeyString(e);
            string windowHex = KeyToHex(e.Key);

            UpdateHotkeysInJson(_hotkeysName, keyString, windowHex);
            this.Close();
        }


        void UpdateHotkeysInJson(string hotkeysName, string newKeyLib, string newWindowKey)
        {
            var baseDir = AppContext.BaseDirectory;
            var settingsPath = Path.Combine(baseDir, "appsettings.json");
            
            string json = File.ReadAllText(settingsPath);

            var obj = JsonObject.Parse(json)?.AsObject();

            if (obj != null && obj["Hotkeys"]?[hotkeysName] != null)
            {
                obj["Hotkeys"]![hotkeysName]!["keyLib"] = newKeyLib;
                obj["Hotkeys"]![hotkeysName]!["windowActionKey"] = newWindowKey;

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                
                string updatedJson = JsonSerializer.Serialize(obj, options);
                File.WriteAllText(settingsPath, updatedJson);
                
                // Recharger les hotkeys
                _hotkeyManager?.ReloadHotkeys();
            }
        }

        private string BuildKeyString(KeyEventArgs e)
        {
            string result = "Ctrl+Shift+";
            result += e.Key.ToString();
            return result;
        }

        private string KeyToHex(Key key)
        {
            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            return "0x" + virtualKey.ToString("X2");
        }
    }
}
