using Microsoft.Data.SqlClient;
using System.Data;

namespace Revalsys.DataAccess
{
    public class InsertReasonDAL
    {
        internal AppDb _db;
        public int CommandTimeout
        {
            get { return _db._CommandTimeout; }
        }
        public InsertReasonDAL(AppDb Db)
        {
            _db = Db;
        }
        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to Insert ReasonDetails.
        //Layer	             :  DAL
        //Method Name        :	InsertReason
        //Input Parameters   :  ReasonName,ReasonCode,ReasonType,ThirdPartyNumber,Description,IsPublished,PrimaryReason,CreatedBy
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	             Md Mujahed Ul Islam    10 Nov 2023        Creation
        //*********************************************************************************************************

        public object InsertReasonDb(dynamic objInserReasonList)
        {
            using (SqlCommand Sqlcmd = _db.connection.CreateCommand())
            {
                _db.connection.Open();
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout = _db._CommandTimeout;
                Sqlcmd.CommandText = "uspInsertRevalReason";
                Sqlcmd.Parameters.Add("@ReasonName", SqlDbType.NVarChar).Value = objInserReasonList.strReasonName;
                Sqlcmd.Parameters.Add("@ReasonCode", SqlDbType.Int).Value = objInserReasonList.intReasonCode;
                Sqlcmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = objInserReasonList.strId;
                Sqlcmd.Parameters.Add("@ThirdPartyNumber", SqlDbType.Int).Value = objInserReasonList.intThirdPartyNumber;
                Sqlcmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = objInserReasonList.strDescription;
                Sqlcmd.Parameters.Add("@IsPublished", SqlDbType.Bit).Value = objInserReasonList.IsPublished;
                Sqlcmd.Parameters.Add("@PrimaryReason", SqlDbType.Bit).Value = objInserReasonList.PrimaryReason;
                Sqlcmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = objInserReasonList.strCreatedBy;
                Sqlcmd.Parameters.Add("@DateCreated", SqlDbType.NVarChar).Value = objInserReasonList.DateCreated;
                object result = Sqlcmd.ExecuteNonQuery();
                _db.connection.Close();
                if (result != null)
                {
                    return result;
                }
                return "Error: objInserReasonList is null";
            }
        }
    }
}
