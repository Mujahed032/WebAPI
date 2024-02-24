using Microsoft.Data.SqlClient;
using System.Data;

namespace Revalsys.DataAccess
{
    public class InsertColorDAL
    {
        internal AppDb _db;
       public int CommandTimeout
        {
            get  { return _db._CommandTimeout; }
        }
        
        public InsertColorDAL(AppDb Db)
        {
            _db = Db;
        }
        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to Insert ColorDetails.
        //Layer	             :  DAL
        //Method Name        :	InsertColor
        //Input Parameters   :  ColorFamily,ColorValue,Description,ColorCodeId,ColorImageId,SwatchColorId,IsPublished,CreatedBy,DateCreated
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	             Md Mujahed Ul Islam    23 Nov 2023        Creation
        //*********************************************************************************************************

        public object InsertColorDb(dynamic objInserReasonList)
        {
            using (SqlCommand Sqlcmd = _db.connection.CreateCommand())
            {
                _db.connection.Open();
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout = _db._CommandTimeout;
                Sqlcmd.CommandText = "uspInsertRevalColor";
                Sqlcmd.Parameters.Add("@ColorFamily", SqlDbType.NVarChar).Value = objInserReasonList.strColorFamily;
                Sqlcmd.Parameters.Add("@ColorValue", SqlDbType.NVarChar).Value = objInserReasonList.strColorValue;
                Sqlcmd.Parameters.Add("@ColorCodeGuid", SqlDbType.NVarChar).Value = objInserReasonList.CstrId;
                Sqlcmd.Parameters.Add("@ColorImageGuid", SqlDbType.NVarChar).Value = objInserReasonList.IstrId;
                Sqlcmd.Parameters.Add("@SwatchColorGuid", SqlDbType.NVarChar).Value = objInserReasonList.SstrId;
                Sqlcmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = objInserReasonList.strDescription;
                Sqlcmd.Parameters.Add("@IsPublished", SqlDbType.Bit).Value = objInserReasonList.IsPublished;
                Sqlcmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = objInserReasonList.strCreatedBy;
                Sqlcmd.Parameters.Add("@DateCreated", SqlDbType.NVarChar).Value = objInserReasonList.DateCreated;
                object result = Sqlcmd.ExecuteScalar();
                _db.connection.Close();

                return result;
               
            }
        }
    }
}
