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
        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to get ErroMessage by ID.
        //Layer	             :  DAL
        //Method Name        :	GetErrorMessageById
        //Input Parameters   :  Id
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	         Md Mujahed Ul Islam        08 Nov 2023        Creation
        //*********************************************************************************************************
        public string GetErrorCode(int errorId)
        {
            using (SqlCommand Sqlcmd = _db.connection.CreateCommand())
            {
                _db.connection.Open();
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout = _db._CommandTimeout;
                Sqlcmd.CommandText = "uspGetErrorCodeRevalReason";
                Sqlcmd.Parameters.Add("@ErrorCode", SqlDbType.Int).Value = errorId;
                object result = Sqlcmd.ExecuteScalar();
                _db.connection.Close();
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
