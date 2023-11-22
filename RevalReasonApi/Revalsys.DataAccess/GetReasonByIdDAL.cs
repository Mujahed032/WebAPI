using Microsoft.Data.SqlClient;
using Revalsys.Properties;
using System.Data;

namespace Revalsys.DataAccess
{
    public class GetReasonByIdDAL
    {
        internal AppDb _db;
        public int CommandTimeout
        {
            get { return _db._CommandTimeout; }
        }
        public GetReasonByIdDAL(AppDb Db)
        {
            _db = Db;
        }
        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to get ReasonDetails by ID.
        //Layer	             :  DAL
        //Method Name        :	GetReasonById
        //Input Parameters   :  Id
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	         Md Mujahed Ul Islam        17 Nov 2023        Creation
        //*********************************************************************************************************

        public List<ReasonSearchResponseListDTO> GetReasonByIdDb(dynamic objReasonList)
        {
            SqlDataAdapter dataAdapter = null;
            DataTable dataTable = null;
            List<ReasonSearchResponseListDTO> lstReasonDetails = null;
            List<DataRow> lstDataRows = null;

            using (SqlCommand Sqlcmd = _db.connection.CreateCommand())
            {
                
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout= _db._CommandTimeout;
                Sqlcmd.CommandText = "uspGetRevalReasonById";
                Sqlcmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = objReasonList.Id;
                dataAdapter = new SqlDataAdapter(Sqlcmd);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                lstDataRows = new List<DataRow>(dataTable.Select());
                if (dataTable != null && dataTable.Rows.Count > 0)
                {

                    lstReasonDetails = new List<ReasonSearchResponseListDTO>();
                    lstReasonDetails = CommonDAL.ConvertToList<ReasonSearchResponseListDTO>(lstDataRows);
                }

                return lstReasonDetails;
            }
        }
    }
}
