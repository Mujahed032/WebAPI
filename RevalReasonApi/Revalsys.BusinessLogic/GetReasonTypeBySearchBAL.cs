using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revalsys.BusinessLogic
{
    public class GetReasonTypeBySearchBAL
    {

        internal AppDb _db;

        public GetReasonTypeBySearchBAL(AppDb appDb)
        {
            _db = appDb;
        }

        public Response<object> GetReasonTypeBySearch()
        {
            Response<object> objResponse = new Response<object>();
            DataTable dataTable = null;
            ErrorCodeDAL errorCodeDAL = null;
            int ErrorCode = 0; 
            string jsonData = null;
            DateTime startResponseTime = DateTime.Now;

            try
            {

                GetReasonTypeBySearchDAL objGetReasonTypeBySearchDAL = new GetReasonTypeBySearchDAL(_db);
                dataTable = objGetReasonTypeBySearchDAL.GetReasonTypeBySearchDb();

                if (dataTable != null)
                {
                    jsonData = JsonConvert.SerializeObject(dataTable);
                    objResponse.ReturnCode = 0;
                    objResponse.ReturnMessage = "Success";
                }
                else
                {
                    ErrorCode = Convert.ToInt32(General.ErrorCode.No_Record_Found);
                }
                if (ErrorCode > 0)
                {
                    errorCodeDAL = new ErrorCodeDAL(_db);
                    objResponse = new Response<object>();
                    objResponse.ReturnCode = ErrorCode;
                    objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
                    objResponse.Data = null;
                }
                else if (ErrorCode == 0 && dataTable != null && dataTable.Rows.Count > 0)
                {
                    objResponse = new Response<object>();
                    objResponse.ReturnCode = ErrorCode;
                    objResponse.ReturnMessage = "Success";
                    objResponse.RecordCount = dataTable.Rows.Count;
                    objResponse.ResponseTime = Math.Round((DateTime.Now - startResponseTime).TotalMilliseconds).ToString();
                    var json = JsonConvert.DeserializeObject<dynamic>(jsonData);
                    objResponse.Data = json;

                }
                objResponse.ResponseTime = Math.Round((DateTime.Now - startResponseTime).TotalMilliseconds).ToString();
            }
            catch (Exception ex)
            {

                objResponse.ReturnCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);
                objResponse.ReturnMessage = $"Exception in GetReasonTypeBySearchBAL: {ex.Message}";
            }

            return objResponse;
        }
    }
}
