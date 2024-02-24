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
    public class DeleteController : ControllerBase
    {
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        internal AppDb _db { get; set; }
        public DeleteController(AppDb appDb, IOptions<AppSetting> configurationSettings, General objGeneral)
        {
            _db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }
        //*********************************************************************************************************
        //Purpose            :  This API is used to Delete ReasonDetails by Id.
        //Layer	             :  API
        //Method Name        :	DeleteReasonDetailsById
        //Input Parameters   :  objAPIRequest
        //Return Values      :  objResponse
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	            Md Mujahed              18 Nov 2023        Creation
        //*********************************************************************************************************
        [HttpPost]
        public async Task<ContentResult> DeleteReasonDetailsById(dynamic objDeleteReason)
        {
            _objGeneral.CreateLog("DeleteController", "DeleteDetailsById", "****** Excution Start ******");
            _objGeneral.CreateLog("DeleteController", "DeleteDetailsById", "step 1 :Request received in DeleteReasonDetailsById Controller");
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
                if (_db != null && objDeleteReason != null)
                {
                    Task<Response<object>> tskResponse = Task<Response<object>>.Run(() =>
                    {
                        DeleteReasonBAL deleteReasonBAL = new(_db,_configurationSettings,_objGeneral);
                        _objGeneral.CreateLog("DeleteControllerr", "DeleteReasonDetailsById", "Step 2 :Request DeleteReasonBAL in Controller");
                        objSearchResponse = deleteReasonBAL.DeleteReason(objDeleteReason);
                        _objGeneral.CreateLog("DeleteController", "DeleteReasonDetailsById", "Step 3 :Response DeleteReasonBAL in Controller");
                        return objSearchResponse;
                    });

                    objDeleteReason = await tskResponse;
                    objResult = objDeleteReason ?? objResponse;
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
            _objGeneral.CreateLog("DeleteController", "DeleteReasonDetailsById", "****** Excutation success ******");
            return objContentResult;
            #endregion
        }
    }
}
