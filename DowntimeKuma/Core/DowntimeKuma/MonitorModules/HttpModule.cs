using System;
using System.Net;

namespace DowntimeKuma.Core.DowntimeKuma.MonitorModules
{
    public class HttpModule : AbstractMonitorModule
    {
        public override string Id => "http";

        public override string DisplayName => "HTTP(S)";

        public override MonitorData Monitor(Monitor m)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.DownloadData(m.Target);

                return new MonitorData()
                {
                    Error = "Successfully queried website via http(s)",
                    Success = true
                };
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
