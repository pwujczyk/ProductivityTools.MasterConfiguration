using ProductivityTools.MasterConfiguration.Builders;
using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.SQL
{
    public class SQLAccess
    {
        public string GetValue(string connectionString, string key, string file, string application,  string schema, string tableName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string query = $"SELECT [Value] from [{schema}].[{tableName}] where [Key]= @key AND [Application]=@application AND [File]=@file";
                SqlCommand sqlComm2 = new SqlCommand(query, sqlConnection);
                sqlComm2.Parameters.AddWithValue("@key", key);
                sqlComm2.Parameters.AddWithValue("@application", application);
                sqlComm2.Parameters.AddWithValue("@file", file);

                sqlConnection.Open();
                var returnValue = sqlComm2.ExecuteScalar();
                if (returnValue != null)
                {
                    return returnValue.ToString();
                }
                else
                {
                    throw new KeyNotFoundException(key);
                }
            }
        }

        public void InsertValueIfNotExists(string connectionString, string schema, string table, ConfigItem config)
        {
            string query = $"IF NOT EXISTS(SELECT [Key] FROM [{schema}].[{table}] WHERE [Key]='{config.Key}' AND [Application]='{config.Application}' AND [File]='{config.File}')" +
              $"  BEGIN " +
              $"      INSERT INTO [{schema}].[{table}]([Key],[Value],[Application],[File],[Category]) VALUES('{config.Key}','{config.Value}','{config.Application}','{config.File}','{config.Category}')" +
              $"  END";
            InvokeSqlQuery(connectionString, query);
        }

        internal void InsertOrUpdateValue(string connectionString, string schema, string table, ConfigItem config)
        {
            string query = $"IF EXISTS(SELECT [Key] FROM [{schema}].[{table}] WHERE [Key]='{config.Key}' AND [Application]='{config.Application}' AND [File]='{config.File}')" +
                $"  BEGIN" +
                $"      UPDATE [{schema}].[{table}] SET [Value]='{config.Value}', [Application]='{config.Application}', [File]='{config.File}', [Category]='{config.Category}' WHERE [Key]='{config.Key}'" +
                $"  END" +
                $" ELSE" +
                $"  BEGIN " +
                $"      INSERT INTO [{schema}].[{table}]([Key],[Value],[Application],[File],[Category]) VALUES('{config.Key}','{config.Value}','{config.Application}','{config.File}','{config.Category}')" +
                $"  END";
            InvokeSqlQuery(connectionString, query);
        }

        public void CreateDatabaseIfNotExists(string dataSourceConnectionString, string databseName)
        {
            string query = string.Format(@"IF NOT EXISTS ( SELECT [Name] FROM sys.databases WHERE [name] = '{0}' )
                            BEGIN
                                CREATE DATABASE {0}
                            END
                            ", databseName);

            InvokeSqlQuery(dataSourceConnectionString, query);
        }

        public void CreateConfigurationTableIfNotExists(string connectionString, string schema, string tableName)
        {
            CreateSchemaIfNotExists(connectionString, schema);
            string query = $@"IF NOT EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}' AND TABLE_SCHEMA = '{schema}')
                            BEGIN
                                CREATE TABLE {schema}.{tableName}
                                (
                                    {tableName}Id INT IDENTITY(1,1),
                                    [Key] VARCHAR(100),
                                    [Value] VARCHAR(1000),
                                    [Application] VARCHAR(40),
                                    [File] VARCHAR(100),
                                    [Category] VARCHAR(100),
                                    CONSTRAINT {tableName}PK PRIMARY KEY ([Key],[Application],[File])
                                )
                            END";
            InvokeSqlQuery(connectionString, query);
        }

        private void CreateSchemaIfNotExists(string connectionString, string schema)
        {
            string query = $@"IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{schema}')
                            BEGIN
                                EXEC('CREATE SCHEMA {schema}')
                            END";
            InvokeSqlQuery(connectionString, query);
        }

        public void InvokeSqlQuery(string connectionString, string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                var returnValue = command.ExecuteNonQuery();
            }
        }
    }
}
