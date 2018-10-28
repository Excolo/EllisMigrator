using System;
using System.Collections.Generic;
using System.Text;

namespace EllisMigrator.SampleMigrations
{
    public class M001_Initial : Migration
    {
        public const string DatabaseName = "MyDatabase";

        public override long Version => 1;

        public override void Up()
        {
            var sql = $@"
                USE Master;
			
                IF EXISTS (SELECT name FROM sys.databases WHERE name='{DatabaseName}')
	                ALTER DATABASE {DatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
			
                DROP DATABASE IF EXISTS {DatabaseName};

                CREATE DATABASE {DatabaseName};
			
                ALTER DATABASE {DatabaseName} SET MULTI_USER;
			
                USE {DatabaseName};
			
                CREATE TABLE [TestTable]
                (
	                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	                Name NVARCHAR(50) NULL,
	                SomeKey NVARCHAR(10) NOT NULL,
	                IsDeleted BIT NOT NULL DEFAULT(0)
                );

                CREATE TABLE [TestRelationTable]
                (
	                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	                TestTableId INT NOT NULL,
	                Description NVARCHAR(MAX) NULL
                );

                ALTER TABLE [TestRelationTable]
                ADD CONSTRAINT FK_TestTable_Id_TestRelationTable_TestTableId FOREIGN KEY (TestTableId)
	                REFERENCES [TestTable] (Id);
	
                ALTER TABLE [TestTable]
                ADD CONSTRAINT AK_TestTable_Name_SomeKey UNIQUE (Name,SomeKey);

                CREATE INDEX IX_TestRelationTable_SomeKey 
	                ON [TestTable] (SomeKey ASC)
	                INCLUDE (IsDeleted);
		    ";

            Execute.RawSql(sql);
        }
    }
}
