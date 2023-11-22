using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Revalsys.Properties;
using Revalsys.BusinessLogic;
using Revalsys.DataAccess;
using Revalsys.Utilities;

namespace RevalReasonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetReasonDetailsController : ControllerBase
    {
        private AppDb _Db { get; set; }

        public GetReasonDetailsController(AppDb appDb)
        {

            _Db = appDb;
        }

        [HttpPost]
        public async Task<ContentResult> Search(dynamic objSearchReasonList)
        {
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
                        getReasonBySearchBAL = new GetReasonBySearchBAL(_Db);
                        objSearchResponse = getReasonBySearchBAL.GetReasonBySearch(objSearchReasonList);
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
