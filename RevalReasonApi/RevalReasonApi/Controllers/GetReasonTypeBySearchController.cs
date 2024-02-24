using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Revalsys.BusinessLogic;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using Serilog;

namespace RevalReasonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetReasonTypeBySearchController : ControllerBase
    {
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        private AppDb _Db { get; set; }

        public GetReasonTypeBySearchController(AppDb appDb,IOptions<AppSetting> configurationSettings,General objGeneral )
        {
            _Db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }
        //*********************************************************************************************************
        //Purpose            :  This API is used to Get ReasonTypeDetails which is dropdown or master table
        //Layer	             :  API
        //Method Name        :	GetReasonTypeDetails
        //Input Parameters   :  objAPIRequest
        //Return Values      :  objResponse
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	             Md Mujahed             20 Nov 2023         Creation
        //*********************************************************************************************************
        [HttpPost]
        public async Task<ContentResult> GetReasonTypeBySearch(dynamic objGetReasonTypeList)
        {
            _objGeneral.CreateLog("GetBySearchontroller", "GetReasonTypeBySearch", "****** Excution start ******");
            _objGeneral.CreateLog("GetBySearchontroller", "GetReasonTypeBySearch", "Step 1 :Request received in GetReasonTypeBySearchController");
            var HeaderType = Request.ContentType;
            Response<object> objSearchResponse = null;
            ContentResult objContentResult = null;
            object objResult = null;
            int StatusCode = 0;
       
            Response<object> objResponse = new Response<object>
            {
                ReturnCode = Convert.ToInt32(General.CommonResponseErrorCodes.InvalidRequest),
                ReturnMessage = Enum.GetName(typeof(General.CommonResponseErrorCodes), General.CommonResponseErrorCodes.InvalidRequest)
            };
       
            try
            {
                if (_Db != null && objGetReasonTypeList != null)
                {
                    Task<Response<object>> tskResponse = Task<Response<object>>.Run(() =>
                    {
                        GetReasonTypeBySearchBAL getReasonTypeBySearchBAL = new(_Db,_configurationSettings,_objGeneral);
                        _objGeneral.CreateLog("GetBySearchontroller", "GetReasonTypeBySearch", "Step 2 :Request GetReasonTypeBySearchBAL in Controller");
                        objSearchResponse = getReasonTypeBySearchBAL.GetReasonTypeBySearch();
                        _objGeneral.CreateLog("GetBySearchontroller", "GetReasonTypeBySearch", "Step 3 :Response GetReasonTypeBySearchBAL in Controller");
                        return objSearchResponse;
                    });

                    objGetReasonTypeList = await tskResponse;
                    objResult = objGetReasonTypeList ?? objResponse;
                }
                else
                {
                    objResult = objResponse;
                }

                StatusCode = (int)General.CommonResponseErrorCodes.Success;
            }
            catch (Exception ex)
            {
                StatusCode = (int)General.CommonResponseErrorCodes.InvalidRequest;
            }
            finally
            {
                objResponse = null;
            }

            objContentResult = new ContentResult() { Content = JsonConvert.SerializeObject(objResult), ContentType = "application/json", StatusCode = StatusCode };
            _objGeneral.CreateLog("GetBySearchontroller", "GetReasonTypeBySearch", "****** Excutation success ******");
            return objContentResult;
        }
    }

}
