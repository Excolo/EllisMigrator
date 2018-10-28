

namespace EllisMigrator
{
    public abstract class Migration
    {
        private IExecutionContext Context { get; set; }

        public abstract long Version { get; }

        public abstract void Up();

        internal void ExecuteUp(IExecutionContext context)
        {
            Context = context;
            Context.PreExecution();

            Up();

            Context.PostExecution();
            Context = null;
        }

        protected IExecutionStatement Execute {
            get { return new RootExecutionStatement(Context); }
        }
    }
}
