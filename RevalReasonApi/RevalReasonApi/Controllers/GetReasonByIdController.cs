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
    public class GetReasonByIdController : ControllerBase
    {
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        private AppDb _Db { get; set; }

        public GetReasonByIdController(AppDb appDb,IOptions<AppSetting> configurationSettings, General objGeneral)
        {

            _Db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }
        [HttpPost]
        //*********************************************************************************************************
        //Purpose            :  This API is used to Get Reason Details by Id.
        //Layer	             :  API
        //Method Name        :	GetReasonDetailsById
        //Input Parameters   :  objAPIRequest
        //Return Values      :  objResponse
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	             Md Mujahed             17 Nov 2023        Creation
        //*********************************************************************************************************
        public async Task<ContentResult> GetReasonById(dynamic objReasonList)
        {
            _objGeneral.CreateLog("GetByIdController", "GetReasonDetailsById", "****** Excution start ******");
            _objGeneral.CreateLog("GetByIdController", "GetReasonDetailsById", "Step 1 :Request received in GetReasonDetailsById Controller");
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
                if (_Db != null && objReasonList != null)
                {
                    Task<Response<object>> tskResponse = Task<Response<object>>.Run(() =>
                    {
                        GetReasonByIdBAL getReasonByIdBAL = new(_Db, _configurationSettings,_objGeneral);
                        _objGeneral.CreateLog("GetByIdController", "GetReasonDetailsById", "Step 2 :Request GetReasonByIdBAL in Controller");
                        objSearchResponse = getReasonByIdBAL.GetReasonById(objReasonList);
                        _objGeneral.CreateLog("GetByIdController", "GetReasonDetailsById", "Step 3 :Response GetReasonByIdBAL in Controller");
                        return objSearchResponse;
                    });

                    objReasonList = await tskResponse;
                    objResult = objReasonList ?? objResponse;

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
                #region Nullifying Objects
                objResponse = null;
                #endregion
            }

            #region output converting xml or json
            objContentResult = new ContentResult() { Content = JsonConvert.SerializeObject(objResult), ContentType = "application/json", StatusCode = StatusCode };
            _objGeneral.CreateLog("GetByIdSearchontroller", "GetReasonDetailsById", "****** Excutation success ******");
            return objContentResult;
            #endregion
        }
    }
}
