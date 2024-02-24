using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Revalsys.Properties;
using Revalsys.BusinessLogic;
using Revalsys.DataAccess;
using Revalsys.Utilities;
using Serilog;
using Microsoft.Extensions.Options;
using Revalsys.Common;

namespace RevalReasonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetReasonDetailsController : ControllerBase
    {
        private AppDb _Db { get; set; }
        private readonly General _objGeneral;
        private readonly IOptions<AppSetting> _objConfigurationSetting;
        public GetReasonDetailsController(AppDb appDb, IOptions<AppSetting> objConfigurationSetting, General objGeneral)
        {
            _objGeneral = objGeneral;
            _objConfigurationSetting = objConfigurationSetting;
            _Db = appDb;
        }
        //*********************************************************************************************************
        //Purpose            :  This API is used to Search ReasonDetails.
        //Layer	             :  API
        //Method Name        :	SearchReasonDetails
        //Input Parameters   :  objAPIRequest
        //Return Values      :  objResponse
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	             Md Mujahed             09 Nov 2023         Creation
        //*********************************************************************************************************

        [HttpPost]
        public async Task<ContentResult> GetReasonDeatilsBySearch(dynamic objSearchReasonList)
        {

            _objGeneral.CreateLog("GetBySearchontroller", "GetReasonDeatilsBySearch", "****** Excution start ******");
            _objGeneral.CreateLog("GetBySearchontroller", "GetReasonDeatilsBySearch", "Step 1 :Request received in GetReasonDeatilsBySearch Controller");
            GetReasonBySearchBAL getReasonBySearchBAL = null;
            var HeaderType = Request.ContentType;
            Response<object> objSearchResponse = null;
            ContentResult objContentResult = null;
            object objResult = null;
            Int32 StatusCode = 0;
            

            
            Response<object> objResponse = new Response<object>
            {
                ReturnCode = Convert.ToInt32(((int)General.CommonResponseErrorCodes.InvalidRequest)),
                ReturnMessage = Enum.GetName(typeof(General.CommonResponseErrorCodes), General.CommonResponseErrorCodes.InvalidRequest)
            };
            
            try
            {
                if (_Db != null && objSearchReasonList != null)
                {
                    Task<Response<object>> tskResponse = Task<Response<object>>.Run(() =>
                    {
                        
                        getReasonBySearchBAL = new GetReasonBySearchBAL(_Db, _objGeneral, _objConfigurationSetting);
                        _objGeneral.CreateLog("GetBySearchontroller", "GetReasonDeatilsBySearch", "Step 2 :Request GetReasonBySearchBAL in Controller");
                        objSearchResponse = getReasonBySearchBAL.GetReasonBySearch(objSearchReasonList);
                        _objGeneral.CreateLog("GetBySearchontroller", "GetReasonDeatilsBySearch", "Step 3 :Response GetReasonBySearchBAL in Controller");
                        return objSearchResponse;
                    });

                    objSearchReasonList = await tskResponse;
                    objResult = objSearchReasonList ?? objResponse;
                }
                else
                {
                    objResult = objResponse;
                }
                StatusCode = (int)General.CommonResponseErrorCodes.Success;
            }
            catch (Exception ex)
            {
                _objGeneral.CreateErrorLog(ex);
                StatusCode = (int)General.CommonResponseErrorCodes.InvalidRequest;
            }
            finally
            {
                #region Nullifying Objects
                objResponse = null;
                #endregion
            }
            #region output converting xml or json
            objContentResult = new ContentResult() { Content = JsonConvert.SerializeObject(objResult), ContentType = "application/json", StatusCode = StatusCode };
            _objGeneral.CreateLog("GetBySearchontroller", "GetReasonDeatilsBySearch", "****** Excutation success ******");
            return objContentResult;
            #endregion


        }
    }
}
