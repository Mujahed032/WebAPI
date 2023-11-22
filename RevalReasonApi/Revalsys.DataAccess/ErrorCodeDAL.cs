using Microsoft.Data.SqlClient;
using System.Data;

namespace Revalsys.DataAccess
{
    public class ErrorCodeDAL
    {
        #region ConnectionString
      
        internal AppDb _db { get; set; }
        public int CommandTimout
        {
            get { return _db._CommandTimeout; }
        }
        public ErrorCodeDAL(AppDb appDb)
        {
          
            _db = appDb;
        }
        #endregion

        public string GetErrorCode(int errorId)
        {
            using (SqlCommand Sqlcmd = _db.connection.CreateCommand())
            {
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout = _db._CommandTimeout;
                Sqlcmd.CommandText = "uspGetErrorCodeRevalReason";
                Sqlcmd.Parameters.Add("@ErrorCode", SqlDbType.Int).Value = errorId;
                object result = Sqlcmd.ExecuteScalar();
                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    return "Error: ErrorMessage is null";
                }
            }

        }
    }
}
