using System.Data;

namespace Revalsys.DataAccess
{
    public class ReasonTypeDAL
    {
        internal AppDb _db;
        public int CommandTimeout
        {
            get { return _db._CommandTimeout; }
        }

        public ReasonTypeDAL(AppDb Db)
        {
            _db = Db;
        }

        public int GetReasonTypeId(string id)
        {

            using (var sqlCmd = _db.connection.CreateCommand())
            {
                _db.connection.Open();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandTimeout = _db._CommandTimeout;
                sqlCmd.CommandText = "uspGetReasonTypeId";
                sqlCmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = id;
                var result = sqlCmd.ExecuteScalar();
                _db.connection.Close();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
