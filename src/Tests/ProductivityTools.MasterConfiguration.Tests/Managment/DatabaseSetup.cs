using ProductivityTools.MasterConfiguration.SQL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Tests.SQL
{
    public class DatabaseSetup
    {
        public const string DataSourceConnectionString = @"Data Source=.\sql2017;Integrated Security=True";
        public const string DatbaseName = "ConfigurationTest";
        public const string Schema = "c";
        public const string Table = "Config";
        public static string ConnectionString = $@"Data Source=.\sql2017;Initial Catalog={DatbaseName};Integrated Security=True";

        public void DropDatabase()
        {
            SQLAccess s = new SQLAccess();
            String sqlCommandText = $@"
                ALTER DATABASE {DatbaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                DROP DATABASE [{DatbaseName}]";
            s.InvokeSqlQuery(DataSourceConnectionString, sqlCommandText);
        }

        public static bool CheckIfDatabaseExists(string connectionString, string databaseName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string query = $"SELECT [Name] FROM sys.databases WHERE [Name] = '{databaseName}'";
                SqlCommand sqlComm2 = new SqlCommand(query, sqlConnection);
                try
                {
                    sqlConnection.Open();
                    var returnValue = sqlComm2.ExecuteScalar();
                    if (returnValue == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
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

     

    }
}
