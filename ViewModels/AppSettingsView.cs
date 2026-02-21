namespace Usely.ViewModels
{
    public class AppSettingsView
    {
        public class HotkeyView
        {
            public string ActionName { get; set; }
            public string KeyLib { get; set; }
            public string WindowActionKey { get; set; }

            public HotkeyView(string actionName, string keyLib, string windowActionKey)
            {
                ActionName = actionName;
                KeyLib = keyLib;
                WindowActionKey = windowActionKey;
            }
        }
    }
}
