using System.Data;
using System.Data.SqlClient;

namespace definer.Core.Connection
{
    public class dbConnection
    {
        private static string? connectionString { get; set; }

        public dbConnection()
        {
            connectionString = "Server=.\\SQLEXPRESS01;Database=definer;User Id=sa;Password=12345a;";
        }

        public static IDbConnection GetConnection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
    }
}