using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Revalsys.BusinessLogic;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;

namespace RevalReasonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateReasonController : ControllerBase
    {
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        internal AppDb _db { get; set; }
        public UpdateReasonController(AppDb appDb, IOptions<AppSetting> configurationSettings,General objGeneral)
        {
            _db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }
        //*********************************************************************************************************
        //Purpose            :  This API is used to Update ReasonDetails by Id.
        //Layer	             :  API
        //Method Name        :	UpdateReasonDetailsById
        //Input Parameters   :  objAPIRequest
        //Return Values      :  objResponse
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	            Md Mujahed              17 Nov 2023        Creation
        //*********************************************************************************************************
        [HttpPost]
        public async Task<ContentResult> UpdateReason(dynamic objUpdateReason)
        {
            _objGeneral.CreateLog("UpdateControllerById", "UpdateReason", "****** Excution Start ******");
            _objGeneral.CreateLog("UpdateControllerById", "UpdateReason", "Step 1 :Request Come to UpdateReasonController ");
            var HeaderType = Request.ContentType;
            Response<object> objUpdateResponse = null;
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
                if (_db != null && objUpdateReason != null)
                {
                    Task<Response<object>> tskResponse = Task<Response<object>>.Run(() =>
                    {
                        UpdateReasonBAL objUpdateReasonBAL = new(_db,_configurationSettings,_objGeneral);
                        _objGeneral.CreateLog("UpdateControllerById", "UpdateReason", "Step 2 : Request UpdateReasonBAL in Controller ");
                        objUpdateResponse = objUpdateReasonBAL.UpdateReason(objUpdateReason);
                        _objGeneral.CreateLog("UpdateControllerById", "UpdateReason", "Step 3 :Response UpdateReasonBAL in Controller ");
                        return objUpdateResponse;
                    });

                    objUpdateReason = await tskResponse;
                    objResult = objUpdateReason ?? objResponse;
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
            _objGeneral.CreateLog("UpdateControllerById", "UpdateReason", "****** Excutation success ******");
            return objContentResult;
            #endregion
        }
    }
}
