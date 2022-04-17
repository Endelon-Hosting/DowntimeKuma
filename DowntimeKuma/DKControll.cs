using DowntimeKuma.Core.Config;
using DowntimeKuma.Core.DowntimeKuma;
using DowntimeKuma.Core.DowntimeKuma.MonitorModules;

using Logging.Net;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Monitor = DowntimeKuma.Core.DowntimeKuma.Monitor;

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

        public static Dictionary<Monitor, MonitorData[]> GetCurrentMonitorStates()
        {
            var result = new Dictionary<Monitor, MonitorData[]>();

            result.Add(new Monitor()
            {
                MonitoringModule = "ping",
                Name = "Testy",
                NotifyModules = new(),
                Target = "test.xyz.de"
            }, new MonitorData[]
            {
                new MonitorData()
                {
                    Error = "Uptimekuma is down",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = true
                },
                new MonitorData()
                {
                    Error = "",
                    Success = true
                },
                new MonitorData()
                {
                    Error = "",
                    Success = true
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                }
            });

            result.Add(new Monitor()
            {
                MonitoringModule = "ping",
                Name = "Uptimekuma",
                NotifyModules = new(),
                Target = "test.xyz.de"
            }, new MonitorData[]
            {
                new MonitorData()
                {
                    Error = "Uptimekuma is down",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                },
                new MonitorData()
                {
                    Error = "",
                    Success = false
                }
            });

            return result;
        }

        public static MonitorData GetLatestMonitorData(Monitor monitor)
        {
            return new MonitorData()
            {
                Success = false,
                Error = ""
            };
        }

        public static AbstractMonitorModule[] GetModules()
        {
            lock(Monitors)
            {
                return new AbstractMonitorModule[]
                {
                    new PingModule()
                };
            }
        }
    }
}
