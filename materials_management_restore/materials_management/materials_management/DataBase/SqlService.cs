using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace materials_management.DataBase
{
    public class SqlService : DatabaseService
    {
        public SqlService(string connectionString)
            : base(connectionString)
        {
            Connection = new SqlConnection(ConnectionString);
            Command = new SqlCommand();
        }
    }
}
