using DowntimeKuma.Core.Config;
using DowntimeKuma.Core.DowntimeKuma;
using Logging.Net;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DowntimeKuma
{
    public class DKControll
    {
        public static Configuration Configuration { get; set; }
        public static ModuleCollection<AbstractMonitorModule> Monitors { get; set; }
        public static ModuleCollection<AbstractNotifyModule> Notifications { get; set; }
        public static List<Core.DowntimeKuma.Monitor> MonitorConfigurations { get; set; }
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
                Thread.Sleep(1000 * 60);
            }
        }
    }
}
