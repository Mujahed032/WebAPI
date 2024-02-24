using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Revalsys.BusinessLogic;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;

namespace RevalColorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertColorController : ControllerBase
    {
        private readonly IOptions<Appsetting> _configurationSettings;
        private readonly General _objGeneral;

        private AppDb _Db { get; set; }

        public InsertColorController(AppDb appDb, IOptions<Appsetting> configurationSettings, General objGeneral)
        {
            _Db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }

        
        
        [HttpPost]
        public async Task<ContentResult> Insertcolor(dynamic objInsertColor)
        {
            _objGeneral.CreateLog("InsertController", "InsertColorDetails", "****** Excution Start ******");
            _objGeneral.CreateLog("InsertController", "InsertColorDetails", "Step 1 :Request Come to InsertColorController ");
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
                if (_Db != null && objInsertColor != null)
                {

                    Task<Response<object>> tskResponse = Task.Run( () =>
                    {
                        _objGeneral.CreateLog("InsertController", "InsertColorDetails", "Step 2 : Request InsertColorBAL in Controller ");
                        InsertColorBAL objInsertColorBAL = new(_Db, _configurationSettings, _objGeneral);
                        objInsertResponse = objInsertColorBAL.InsertColor(objInsertColor);
                        _objGeneral.CreateLog("InsertController", "InsertColorDetails", "Step 3 :Response InsertColorBAL in Controller ");
                        return objInsertResponse;
                    });

                    objInsertColor = await tskResponse;
                    objResult = objInsertColor ?? objResponse;
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
            _objGeneral.CreateLog("InsertController", "InsertColorDetails", "****** Excutation success ******");
            return objContentResult;
            #endregion

        }
    }
}

