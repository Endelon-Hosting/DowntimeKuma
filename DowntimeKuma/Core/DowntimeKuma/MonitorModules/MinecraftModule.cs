using MineStatLib;

using System;

namespace DowntimeKuma.Core.DowntimeKuma.MonitorModules
{
    public class MinecraftModule : AbstractMonitorModule
    {
        public override string Id => "mc";

        public override string DisplayName => "Minecraft Server Ping";

        public override MonitorData Monitor(Monitor m)
        {
            try
            {
                var parts = m.Target.Split(":");

                var host = parts[0];
                var port = (ushort)25565;

                if (parts.Length > 1)
                    port = ushort.Parse(parts[1]);

                var mc = new MineStat(host, port);

#pragma warning disable CS0612 // Typ oder Element ist veraltet
                if (mc.IsServerUp())
                {
                    return new MonitorData()
                    {
                        Error = "Server is online",
                        Success = true
                    };
                }
                else
                {
                    return new MonitorData()
                    {
                        Error = "Server is offline",
                        Success = false
                    };
                }
#pragma warning restore CS0612 // Typ oder Element ist veraltet
            }
            catch(Exception e)
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
            
        }
    }
}
