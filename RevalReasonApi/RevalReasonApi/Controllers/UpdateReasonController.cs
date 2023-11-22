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
    public class UpdateReasonController : ControllerBase
    {
        internal AppDb _db { get; set; }
        public UpdateReasonController(AppDb appDb)
        {
            _db = appDb;
        }
        [HttpPost]
        public async Task<ContentResult> Update(dynamic objUpdateReason)
        {

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
                        UpdateReasonBAL objUpdateReasonBAL = new(_db);
                        objUpdateResponse = objUpdateReasonBAL.UpdateReason(objUpdateReason);
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
            return objContentResult;
            #endregion
        }
    }
}
