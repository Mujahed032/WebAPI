using Microsoft.Data.SqlClient;
using System.Data;

namespace Revalsys.DataAccess
{
    public class UpdateReasonDAL
    {
        internal AppDb _db;
        public int CommandTimeout
        {
            get { return _db._CommandTimeout; }
        }
        public UpdateReasonDAL(AppDb Db)
        {
            _db = Db;
        }
        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to Update ReasonDetails.
        //Layer	             :  DAL
        //Method Name        :	UpdateReason
        //Input Parameters   :  ReasonName,ReasonCode,ReasonTypeId,ThirdPartyNumber,Description,IsPublished,PrimaryReason,UpdatedBy
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	        Md Mujahed Ul Islam         17 Nov 2023        Creation
        //*********************************************************************************************************
        public string UpdateReasonDb(dynamic objUpdateReason)
        {
            using (SqlCommand Sqlcmd = _db.connection.CreateCommand())
            {
                _db.connection.Open();
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout = _db._CommandTimeout;
                Sqlcmd.CommandText = "uspUpdateRevalReasonById";
                if (objUpdateReason != null)
                {
                    Sqlcmd.Parameters.Add("@ReasonId", SqlDbType.VarChar).Value = objUpdateReason.intReasonId;
                    Sqlcmd.Parameters.Add("@ReasonName", SqlDbType.VarChar).Value = objUpdateReason.strReasonName;
                    Sqlcmd.Parameters.Add("@ReasonCode", SqlDbType.Int).Value = objUpdateReason.intReasonCode;
                    Sqlcmd.Parameters.Add("@RId", SqlDbType.Int).Value = objUpdateReason.intReasonTypeId;
                    Sqlcmd.Parameters.Add("@ThirdPartyNumber", SqlDbType.Int).Value = objUpdateReason.intThirdPartyNumber;
                    Sqlcmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = objUpdateReason.strDescription;
                    Sqlcmd.Parameters.Add("@PrimaryReason", SqlDbType.Bit).Value = objUpdateReason.PrimaryReason;
                    Sqlcmd.Parameters.Add("@IsPublished", SqlDbType.Bit).Value = objUpdateReason.IsPublished;
                    Sqlcmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = objUpdateReason.strUpdatedBy;
                    Sqlcmd.Parameters.Add("@LastUpdated", SqlDbType.NVarChar).Value = objUpdateReason.LastUpdated;
                    object result = Sqlcmd.ExecuteScalar();
                    _db.connection.Close();
                    return result?.ToString() ?? string.Empty;
                }
                else
                {
                    return "Error: objUpdateReason is null";
                }
            }

        }
    }
}