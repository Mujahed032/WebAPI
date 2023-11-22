using Microsoft.Data.SqlClient;
using System.Data;

namespace Revalsys.DataAccess
{
    public class GetReasonTypeBySearchDAL
    {
        #region ConnectionString
        internal AppDb _Db { get; set; }
        public int CommandTimeout
        {
            get { return _Db._CommandTimeout; }
        }
        public GetReasonTypeBySearchDAL(AppDb Db)
        {
            _Db = Db;
        }
        #endregion

        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to ReasonTypeDetails.
        //Layer	             :  DAL
        //Method Name        :	GetReasonTypeBySearch
        //Input Parameters   :  ReasonType
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	          Md Mujahed Ul Islam       20 Nov 2023        Creation
        //*********************************************************************************************************

        public DataTable GetReasonTypeBySearchDb()
        {

            using (SqlCommand Sqlcmd = _Db.connection.CreateCommand())
            {
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout = _Db._CommandTimeout;
                Sqlcmd.CommandText = "uspGetRevalReasonTypeBySearch";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(Sqlcmd);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return dataTable;
                }
                else
                {
                    throw new Exception("No Record Found");
                }
            }

        }
    }
}

