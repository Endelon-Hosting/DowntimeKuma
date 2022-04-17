using DowntimeKuma.Core.Config;

using Logging.Net;

using Newtonsoft.Json;

using System;
using System.IO;

namespace DowntimeKuma
{
    public class DKControll
    {
        public static Configuration Configuration { get; set; }

        public static void Start()
        {
            try
            {
                Logger.Info("Starting DowntimeKuma v0.1");

                Logger.Info("Loading configuration file 'config.json'");
                Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));
            }
            catch(Exception e)
            {
                Logger.Error("Failed to start DowntimeKuma");
                Logger.Error(e);

                Environment.Exit(1);
            }
        }
    }
}
