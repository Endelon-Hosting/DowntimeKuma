using Logging.Net;

using System;
using System.Net;
using System.Net.NetworkInformation;

namespace DowntimeKuma.Core.DowntimeKuma.MonitorModules
{
    public class PingModule : AbstractMonitorModule
    {
        public override string Id { get { return "ping"; } }

        public override MonitorData Monitor(Monitor m)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(m.Target);

                if (reply.Status == IPStatus.Success)
                {
                    return new MonitorData()
                    {
                        Success = true,
                        Error = $"Ping: {reply.RoundtripTime}ms"
                    };
                }
                else
                    return new MonitorData()
                    {
                        Error = $"Ping failed: {reply.Status}",
                        Success = false
                    };
            }
            catch (Exception e)
            {
                return new MonitorData()
                {
                    Error = e.Message,
                    Success = false
                };
            }
        }

        public override void Prepare()
        {
            Logger.Info($"Loaded '{Id}' module");
        }
    }
}