using EllisMigrator.Runner.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EllisMigrator.Runner
{
    public class MigrationRunner
    {
        private List<Migration> _migrations = new List<Migration>();
        private readonly IExecutionContext context;

        public MigrationRunner(IExecutionContext context)
        {
            this.context = context;
        }

        public void AddMigrationsFromAssembly<TAssembly>()
            => AddMigrationsFromAssembly(typeof(TAssembly).Assembly);

        public void AddMigrationsFromAssembly(Assembly migrationAssembly)
        {
            var migrationClasses = migrationAssembly
                .GetTypes()
                .AsEnumerable();

            migrationClasses = migrationClasses.Where(t => typeof(Migration).IsAssignableFrom(t));

            var migrations = migrationClasses.Select(t => (Migration)Activator.CreateInstance(t));
            AddMigrations(migrations.ToArray());
        }

        public void AddMigration<TMigration>()
            where TMigration : Migration
        {
            _migrations.Add(Activator.CreateInstance<TMigration>());

            ValidateMigrationVersions();
        }

        public void AddMigrations(params Migration[] migrations)
        {
            _migrations.AddRange(migrations);

            ValidateMigrationVersions();
        }

        public void Run()
            => Run(0, long.MaxValue);

        public void Run(long toVersion)
            => Run(0, toVersion);

        public void Run(long fromVersion, long toVersion)
        {
            var migrationsToRun = _migrations
                .Where(m => m.Version > fromVersion && m.Version <= toVersion)
                .OrderBy(m => m.Version);

            foreach(var migration in migrationsToRun)
            {
                migration.ExecuteUp(context);
            }
        }

        private void ValidateMigrationVersions()
        {
            var hasDuplicateVersions = _migrations
                .GroupBy(m => m.Version)
                .Any(m => m.Count() > 1);

            if (hasDuplicateVersions)
            {
                throw new MigrationException("Migration runner found duplicate migration versions.");
            }
        }
    }
}
