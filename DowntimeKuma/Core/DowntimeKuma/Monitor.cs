using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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

        public string Now()
        {
            var n = DateTime.Now;
            return $"{n.Day}.{n.Month}.{n.Year}";
        }

        public string DailyStatsPath()
        {
            return $"data/monitors/{Name.ToLower().Replace(" ", "_")}-{MonitoringModule.ToLower().Replace(" ", "_")}/history/{Now()}.json";
        }

        public string XD(string fn)
        {
            var dir = Path.GetDirectoryName(fn);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return fn;
        }

        public string JX(string fn, string m = "{}")
        {
            if (!File.Exists(fn))
                File.WriteAllText(fn, m);
            return fn;
        }

        public MonitorData[] GetHistory()
        {
            return JsonConvert.DeserializeObject<MonitorData[]>(File.ReadAllText(JX(XD(DailyStatsPath()), "[]")));
        }
    }
}
