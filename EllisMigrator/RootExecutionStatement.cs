namespace EllisMigrator
{
    public class RootExecutionStatement : IExecutionStatement
    {
        public RootExecutionStatement(IExecutionContext context)
        {
            Context = context;
        }

        public IExecutionContext Context { get; }
    }
}
