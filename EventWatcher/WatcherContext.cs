using EventWatcher.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventWatcher
{
    public class WatcherContext : ApplicationContext
    {
        NotifyIcon ni;

        public WatcherContext()
        {
            Setup();
            ObserveEventLog();
        }

        private void Setup()
        {
            ni = new NotifyIcon();
            ni.Icon = Resources.AppIcon;

            var menu = new ContextMenuStrip()
            {
                Text = "EventLog Watcher",
            };
            menu.Items.Add("Exit", null, Exit);
            ni.ContextMenuStrip = menu;

            ni.Visible = true;
        }

        private void ObserveEventLog()
        {
            var log = new EventLog("EventWatcherTest");
            log.EnableRaisingEvents = true;
            log.EntryWritten += Log_EntryWritten;
        }

        private void Log_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (e.Entry.EntryType <= UserSettings.Current.MinimumSeverity)
            {
                ni.BalloonTipTitle = "New eventlog entry!";
                ni.BalloonTipText = e.Entry.Message;

                switch (e.Entry.EntryType)
                {
                    case EventLogEntryType.Error:
                        ni.BalloonTipIcon = ToolTipIcon.Error;
                        break;
                    case EventLogEntryType.Warning:
                        ni.BalloonTipIcon = ToolTipIcon.Warning;
                        break;
                    case EventLogEntryType.Information:
                    case EventLogEntryType.SuccessAudit:
                    case EventLogEntryType.FailureAudit:
                    default:
                        ni.BalloonTipIcon =  ToolTipIcon.Info;
                        break;
                }

                ni.ShowBalloonTip(3000);
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            ni.Visible = false;
            Application.Exit();
        }
    }
}
