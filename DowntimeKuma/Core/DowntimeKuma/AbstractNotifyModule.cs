namespace DowntimeKuma.Core.DowntimeKuma
{
    public abstract class AbstractNotifyModule : AbstractModule
    {
        public abstract void Notify(Notification notification);
    }
}