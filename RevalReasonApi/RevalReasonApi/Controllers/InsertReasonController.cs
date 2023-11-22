using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Revalsys.BusinessLogic;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;

namespace RevalReasonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertReasonController : ControllerBase
    {
        private AppDb _Db { get; set; }

        public InsertReasonController(AppDb appDb)
        {

            _Db = appDb;
        }
        [HttpPost]
        public async Task<ContentResult> Insert(dynamic objInsertReason)
        {

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
                    //++1
                    Task<Response<object>> tskResponse = Task<Response<object>>.Run(() =>
                    {
                        InsertReasonBAL objInsertReasonBAL = new(_Db);
                        objInsertResponse = objInsertReasonBAL.InsertReason(objInsertReason);
                        return objInsertResponse;
                    });
                    //++
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
                StatusCode = (int)General.CommonResponseErrorCodes.InvalidRequest;
            }
            finally
            {
                objResponse = null;
            }

            #region output converting xml or json
            objContentResult = new ContentResult() { Content = JsonConvert.SerializeObject(objResult), ContentType = "application/json", StatusCode = StatusCode };
            return objContentResult;
            #endregion

        }
    }
}
