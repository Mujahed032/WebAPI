using Microsoft.Data.SqlClient;

namespace Revalsys.DataAccess
{
    public class AppDb : IDisposable
    {
        public SqlConnection connection { get; }
        public int _CommandTimeout;
        public AppDb(string connectionString,int CommandTimeout)
        {
            connection = new SqlConnection(connectionString);
            _CommandTimeout= CommandTimeout;
        }

        public void Dispose() => connection.Dispose();
    }
}