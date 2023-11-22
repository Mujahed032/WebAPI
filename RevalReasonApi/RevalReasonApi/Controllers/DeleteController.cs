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
    public class DeleteController : ControllerBase
    {
        internal AppDb _db { get; set; }
        public DeleteController(AppDb appDb)
        {
            _db = appDb;
        }
        [HttpPost]
        public async Task<ContentResult> Delete(dynamic objDeleteReason)
        {

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
                        DeleteReasonBAL deleteReasonBAL = new(_db);
                        objSearchResponse = deleteReasonBAL.DeleteReason(objDeleteReason);
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
            catch (Exception )
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
            return objContentResult;
            #endregion
        }
    }
}
