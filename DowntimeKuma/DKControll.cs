using DowntimeKuma.Core.Config;
using DowntimeKuma.Core.DowntimeKuma;
using DowntimeKuma.Core.DowntimeKuma.MonitorModules;
using Logging.Net;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Monitor = DowntimeKuma.Core.DowntimeKuma.Monitor;

namespace DowntimeKuma
{
    public class DKControll
    {
        public static Configuration Configuration { get; set; }
        public static ModuleCollection<AbstractMonitorModule> Monitors { get; set; }
        public static ModuleCollection<AbstractNotifyModule> Notifications { get; set; }
        public static List<Monitor> MonitorConfigurations { get; set; }
        public static Thread Monitoring { get; set; } = null;

        public static void Start()
        {
            try
            {
                Logger.Info("Starting DowntimeKuma v0.1");

                Logger.Info("Loading configuration file 'config.json'");
                Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));

                Logger.Info("Registering modules");
                Monitors = new();
                Monitors.Add(new PingModule());
                Notifications = new();
                MonitorConfigurations = new();

                Logger.Info("Starting Monitor Thread");
                Monitoring = new(MonitorThread);
                Monitoring.Start();
            }
            catch(Exception e)
            {
                Logger.Error("Failed to start DowntimeKuma");
                Logger.Error(e);

                Environment.Exit(1);
            }
        }

        private static void MonitorThread()
        {
            while (true)
            {
                foreach (var mon in MonitorConfigurations)
                {
                    mon.AddToHistory(mon.GetMonitoringService().Monitor(mon));
                }
                Thread.Sleep(1000 * 60);
            }
        }

        public static Dictionary<Monitor, MonitorData[]> GetCurrentMonitorStates()
        {
            var result = new Dictionary<Monitor, MonitorData[]>();

            foreach (var monitor in MonitorConfigurations)
            {
                result.Add(monitor, monitor.GetHistory());
            }

            return result;
        }

        public static MonitorData GetLatestMonitorData(Monitor monitor)
        {
            return monitor.GetHistory().Last();
        }

        public static AbstractMonitorModule[] GetModules()
        {
            lock(Monitors)
            {
                return Monitors.ToArray();
            }
        }

        public static void AddMonitor(Monitor monitor)
        {
            lock(MonitorConfigurations)
            {
                MonitorConfigurations.Add(monitor);
            }
        }

        public static void UpdateMonitor(Monitor monitor)
        {
            lock(MonitorConfigurations)
            {
                var select = MonitorConfigurations.Find(x => x.Id == monitor.Id);

                select.NotifyModules = monitor.NotifyModules;
                select.MonitoringModule = monitor.MonitoringModule;
                select.Name = monitor.Name;
            }
        }
    }
}