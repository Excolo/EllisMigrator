using System;
using System.Collections.Generic;
using System.Text;

namespace EllisMigrator.SampleMigrations
{
    public class M002_AlterTable : Migration
    {
        public override long Version => 2;

        public override void Up()
        {
            var sql = $@"
                ALTER TABLE [TestTable]
                    ADD Timestamp DATETIME2 NOT NULL DEFAULT GETDATE();
            ";

            Execute.RawSql(sql);
        }
    }
}
