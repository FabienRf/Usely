namespace Usely.Models
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
