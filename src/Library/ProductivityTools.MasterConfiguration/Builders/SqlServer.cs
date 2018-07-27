using ProductivityTools.MasterConfiguration.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Builders
{
    class SqlServer : IBuilder
    {
        private readonly SQLAccess DataAccess;
        private readonly string ConnectionString;
        private readonly string Schema;
        private readonly string TableName;


        public SqlServer(string connectionString, string schema, string tableName)
        {
            this.DataAccess = new SQLAccess();
            this.ConnectionString = connectionString;
            this.Schema = schema;
            this.TableName = tableName;

        }

        //pw: todo
        public Dictionary<string, string> GetAllValues()
        {
            return new Dictionary<string, string>();
        }

        public string GetValue(string key)
        {
            var r = this.DataAccess.GetValue(ConnectionString, key, Schema, TableName);
            return r;
        }

        public void InsertOrUpdate(string key, string value)
        {
            this.DataAccess.InsertOrUpdateValue(ConnectionString, Schema, TableName, key, value);
        }

        public void InsertIfNotExists(string key, string value)
        {
            this.DataAccess.InsertValueIfNotExists(ConnectionString, Schema, TableName, key, value);
        }
        
    }
}
