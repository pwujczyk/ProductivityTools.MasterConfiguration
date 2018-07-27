using ProductivityTools.MasterConfiguration.Builders;
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
        public Dictionary<string,string> GetAllValues()
        {
            //pw: todo
            throw new Exception();
        }

        public string GetValue(string connectionString,string key,string schema, string tableName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string query = $"SELECT [Value] from [{schema}].[{tableName}] where [Key]= @keyId";
                SqlCommand sqlComm2 = new SqlCommand(query, sqlConnection);
                sqlComm2.Parameters.AddWithValue("@keyId", key);
                try
                {
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
                catch (Exception ex)
                {
                    //pw: add here
                    throw ex;
                }
                //pw: correctit
                throw new Exception();
            }
        }

        public void InsertValueIfNotExists(string connectionString, string schema, string table, string key, string value)
        {
            string query = $"IF NOT EXISTS(SELECT [Key] FROM [{schema}].[{table}] WHERE [Key]='{key}')" +
              $"  BEGIN " +
              $"      INSERT INTO [{schema}].[{table}]([Key],[Value]) VALUES('{key}','{value}')" +
              $"  END";
            InvokeSqlQuery(connectionString, query);
        }

        internal void InsertOrUpdateValue(string connectionString, string schema, string table,  string key, string value)
        {
            string query = $"IF EXISTS(SELECT [Key] FROM [{schema}].[{table}] WHERE [Key]='{key}')" +
                $"  BEGIN" +
                $"      UPDATE [{schema}].[{table}] SET [Value]='{value}' WHERE [Key]='{key}'" +
                $"  END" +
                $" ELSE" +
                $"  BEGIN " +
                $"      INSERT INTO [{schema}].[{table}]([Key],[Value]) VALUES('{key}','{value}')" +
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
                                    {tableName}Id INT IDENTITY(1,1) PRIMARY KEY,
                                    [Key] VARCHAR(30),
                                    [Value] VARCHAR(200)
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



        public void InvokeSqlQuery(string connectionString,string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, sqlConnection);
                try
                {
                    sqlConnection.Open();
                    var returnValue = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //pw: add here
                    throw ex;
                }
                //pw: correctit
            }

        }
    }
}
