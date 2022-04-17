using System.Collections.Generic;

namespace DowntimeKuma.Core.DowntimeKuma
{
    public class Monitor
    {
        public string MonitoringModule { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public List<string> NotifyModules { get; set; } = new();

        public AbstractMonitorModule GetMonitoringService()
        {
            return DKControll.Monitors.Find(x => x.Id == MonitoringModule);
        }
    }
}
