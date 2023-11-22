using Microsoft.Data.SqlClient;
using Revalsys.Properties;
using System.Data;

namespace Revalsys.DataAccess
{
    public class GetReasonBySearchDAL
    {
        #region ConnectionString
        internal AppDb _Db { get; set; }
        public int CommandTimeout
        {
            get { return _Db._CommandTimeout; }
        }
        public GetReasonBySearchDAL(AppDb Db)
        {
            _Db = Db;

        }
        #endregion

        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to ReasonDetails.
        //Layer	             :  DAL
        //Method Name        :	GetReasonBySearch
        //Input Parameters   :  ReasonName ,PageSize
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	          Md Mujahed Ul Islam       09 Nov 2023        Creation
        //*********************************************************************************************************

        public List<ReasonSearchResponseListDTO> GetReasonBySearchDb(dynamic objGetReasonList)
        {
            DataTable dtResponce = null;
            List<ReasonSearchResponseListDTO> lstReasonDetails = null;
            SqlDataAdapter sqlDataAdapter = null;
            List<DataRow> lstDataRows = null;

            using (SqlCommand Sqlcmd = _Db.connection.CreateCommand())
            {
                _Db.connection.Open();
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.CommandTimeout = _Db._CommandTimeout;
                Sqlcmd.CommandText = "uspGetRevalReasonBySearch";
                Sqlcmd.Parameters.Add("@ReasonName", SqlDbType.NVarChar).Value = objGetReasonList.ReasonName;
                int pageSize = Convert.ToInt32(objGetReasonList.PageSize);
                int pageNumber = Convert.ToInt32(objGetReasonList.PageNumber);
                Sqlcmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                Sqlcmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                sqlDataAdapter = new SqlDataAdapter(Sqlcmd);
                dtResponce = new DataTable();
                sqlDataAdapter.Fill(dtResponce);
                if (dtResponce != null && dtResponce.Rows.Count > 0)                 
                {
                    
                    lstReasonDetails = new List<ReasonSearchResponseListDTO>();
                    lstDataRows = new List<DataRow>(dtResponce.Select());
                    lstReasonDetails = CommonDAL.ConvertToList<ReasonSearchResponseListDTO>(lstDataRows);
                }
            }

            return lstReasonDetails;
        }
    }
}

