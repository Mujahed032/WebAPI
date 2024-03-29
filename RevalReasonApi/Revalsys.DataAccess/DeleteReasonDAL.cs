﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace Revalsys.DataAccess
{
    public class DeleteReasonDAL
    {
        internal AppDb _db;
        public int CommandTimeOut
        {
            get { return _db._CommandTimeout; }
        }
        public DeleteReasonDAL(AppDb appDb)
        {
            _db = appDb;
        }
        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to Delete ReasonDetails.
        //Layer	             :  DAL
        //Method Name        :	DeleteReason
        //Input Parameters   :  Id,DeletedBy
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  ------------------------------------------------------------------------------------------------------
        //    1.0	          Md Mujahed Ul Islam       18 Nov 2023        Creation
        //*********************************************************************************************************


        public string DeleteReasonDb(dynamic objDeleteReason)
        {

            using (SqlCommand Sqlcmd = _db.connection.CreateCommand())
            {
                _db.connection.Open();
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout = _db._CommandTimeout;
                Sqlcmd.CommandText = "uspDeleteRevalReason";
                Sqlcmd.Parameters.Add("@RevalReasonId", SqlDbType.Int).Value = objDeleteReason.RevalReasonId;
                Sqlcmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar).Value = objDeleteReason.DeletedBy;
                Sqlcmd.Parameters.Add("@DateDeleted", SqlDbType.NVarChar).Value = objDeleteReason.DateDeleted;
                object result = Sqlcmd.ExecuteNonQuery();
                _db.connection.Close();
                if (result != null)
                {
                    return result.ToString();
                }
            }


            return "Error: objDeleteReason is null";
        }
    }
}
