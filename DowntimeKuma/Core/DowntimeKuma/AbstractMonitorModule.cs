namespace DowntimeKuma.Core.DowntimeKuma
{
    public abstract class AbstractMonitorModule : AbstractModule
    {
        public abstract MonitorData Monitor();
    }
}
