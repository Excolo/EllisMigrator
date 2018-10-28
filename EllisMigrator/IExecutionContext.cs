namespace EllisMigrator
{
    public interface IExecutionContext
    {
        void PreExecution();

        void PostExecution();
    }

    public abstract class BaseExecutionContext : IExecutionContext
    {
        public virtual void PostExecution()
        { }

        public virtual void PreExecution()
        { }
    }
}
