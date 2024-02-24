using Microsoft.AspNetCore.Http;
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
    public class InsertReasonController : ControllerBase
    {
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        private AppDb _Db { get; set; }

        public InsertReasonController(AppDb appDb, IOptions<AppSetting> configurationSettings, General objGeneral)
        {
            _Db = appDb;
            _objGeneral = objGeneral;
            _configurationSettings = configurationSettings;
        }
        //*********************************************************************************************************
        //Purpose            :  This API is used to Insert ReasonDetails.
        //Layer	             :  API
        //Method Name        :	InsertReasonDetails
        //Input Parameters   :  objAPIRequest
        //Return Values      :  objResponse
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  -------------------------------------------------------------------------------------------------------
        //    1.0	             Md Mujahed             10 Nov 2023         Creation
        //*********************************************************************************************************
        [HttpPost]
        public async Task<ContentResult> InsertReason(dynamic objInsertReason)
        {
            _objGeneral.CreateLog("InsertController", "InsertReasonDetails", "****** Excution Start ******");
            _objGeneral.CreateLog("InsertController", "InsertReasonDetails", "Step 1 :Request Come to InsertReasonController ");
            var HeaderType = Request.ContentType;
            Response<object> objInsertResponse = null;
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
                if (_Db != null && objInsertReason != null)
                {

                    Task<Response<object>> tskResponse = Task<Response<object>>.Run(async () =>
                    {
                        InsertReasonBAL objInsertReasonBAL = new(_Db, _configurationSettings, _objGeneral);
                        _objGeneral.CreateLog("InsertController", "InsertReasonDetails", "Step 2 : Request InsertReasonBAL in Controller ");
                        objInsertResponse = objInsertReasonBAL.InsertReason(objInsertReason);
                        _objGeneral.CreateLog("InsertController", "InsertReasonDetails", "Step 3 :Response InsertReasonBAL in Controller ");
                        return objInsertResponse;
                    });

                    objInsertReason = await tskResponse;
                    objResult = objInsertReason ?? objResponse;
                }
                else
                {
                    objResult = objResponse;
                }
                StatusCode = (int)General.CommonResponseErrorCodes.Success;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
                StatusCode = (int)General.CommonResponseErrorCodes.InvalidRequest;
            }
            finally
            {
                objResponse = null;
            }

            #region output converting xml or json
            objContentResult = new ContentResult() { Content = JsonConvert.SerializeObject(objResult), ContentType = "application/json", StatusCode = StatusCode };
            _objGeneral.CreateLog("InsertController", "InsertReasonDetails", "****** Excutation success ******");
            return objContentResult;
            #endregion

        }
    }
}
