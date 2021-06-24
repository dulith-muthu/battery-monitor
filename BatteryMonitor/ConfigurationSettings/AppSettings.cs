using System;
using System.Collections.Generic;
using System.Text;

namespace BatteryMonitor.ConfigurationSettings
{
    class AppSettings
    {
        public const string Position = "AppSettings";

        public int CheckDelay { get; set; }

        public List<string> BatteryProperties { get; set; }

        public string BatteryClass { get; set; }

        public string CheckPluggedIn { get; set; }

        public string AppId { get; set; }

        public string ToastMessage { get; set; }
    }
}
