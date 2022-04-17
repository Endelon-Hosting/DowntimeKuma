namespace DowntimeKuma.Core.DowntimeKuma
{
    public abstract class AbstractModule
    {
        public abstract void Prepare();
        public abstract string Id { get; }
    }
}
