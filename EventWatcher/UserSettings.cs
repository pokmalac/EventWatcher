using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventWatcher
{
    public class UserSettings
    {

        public string LogName { get; set; }
        public EventLogEntryType MinimumSeverity { get; set; }

        private static UserSettings _instance;
        private static volatile object sync = new object();
        public static UserSettings Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserSettings();
                        }
                    }
                }

                return _instance;
            }
        }

        private UserSettings()
        {
            LogName = System.Configuration.ConfigurationManager.AppSettings["LogName"];
            string minimumSeverityLevelName = ConfigurationManager.AppSettings["MinimumSeverityLevel"];
            EventLogEntryType minsev = EventLogEntryType.Warning;
            Enum.TryParse<EventLogEntryType>(minimumSeverityLevelName, out minsev);
            MinimumSeverity = minsev;
        }

    }
}
