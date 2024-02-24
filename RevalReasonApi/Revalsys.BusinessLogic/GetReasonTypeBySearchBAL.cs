using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using System.Data;

namespace Revalsys.BusinessLogic
{
    public class GetReasonTypeBySearchBAL
    {

        internal AppDb _db;
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        public GetReasonTypeBySearchBAL(AppDb appDb,IOptions<AppSetting> configurationSettings,General objGeneral)
        {
            _db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }
        /*
        * Author Name            :  Md Mujahed Ul Islam 
        * Create Date            :  20 Nov 2023
        * Modified Date          : 
        * Modified Reason        : 
        * Layer                  :  BAL
        * Modified By            : 
        * Description            :  This class have the Reason Module Business Logic Code To get ReasonTypeBySearch.
        */
        public Response<object> GetReasonTypeBySearch()
        {
            _objGeneral.CreateLog("GetBySearchBAL", "GetReasoTypeBySearch", "Step 2.1 :Request received in GetReasonTypeBySearch BAL");
            Response<object> objResponse = new Response<object>();
            DataTable dataTable = null;
            ErrorCodeDAL errorCodeDAL = null;
            int ErrorCode = 0; 
            string jsonData = null;
            DateTime startResponseTime = DateTime.Now;

            try
            {

                GetReasonTypeBySearchDAL objGetReasonTypeBySearchDAL = new GetReasonTypeBySearchDAL(_db);
                _objGeneral.CreateLog("GetBySearchBAL", "GetReasonTypeBySearch", "Step 2.2 :Request GetReasonBySearchDb in BAL");
                dataTable = objGetReasonTypeBySearchDAL.GetReasonTypeBySearchDb();
                _objGeneral.CreateLog("GetBySearchBAL", "GetReasonTypeBySearch", "Step 2.3 :Response GetReasonBySearchDb in BAL");
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
                    _objGeneral.CreateLog("GetBySearchBAL", "GetReasonTypeBySearch", "Step 2.4 :Request GetErrorCode in BAL");
                    objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
                    _objGeneral.CreateLog("GetBySearchBAL", "GetReasonTypeBySearch", "Step 2.5 :Response GetErrorCode in BAL");
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
