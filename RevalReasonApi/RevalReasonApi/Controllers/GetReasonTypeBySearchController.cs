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
    public class GetReasonTypeBySearchController : ControllerBase
    {
        private AppDb _Db { get; set; }

        public GetReasonTypeBySearchController(AppDb appDb)
        {
            _Db = appDb;
        }

        [HttpPost]
        public async Task<ContentResult> GetReasonTypeBySearch(dynamic objGetReasonTypeList)
        {
            var HeaderType = Request.ContentType;
            Response<object> objSearchResponse = null;
            ContentResult objContentResult = null;
            object objResult = null;
            int StatusCode = 0;

            Response<object> objResponse = new Response<object>
            {
                ReturnCode = Convert.ToInt32(General.CommonResponseErrorCodes.InvalidRequest),
                ReturnMessage = Enum.GetName(typeof(General.CommonResponseErrorCodes), General.CommonResponseErrorCodes.InvalidRequest)
            };

            try
            {
                if (_Db != null && objGetReasonTypeList != null)
                {
                    Task<Response<object>> tskResponse = Task<Response<object>>.Run(() =>
                    {
                        GetReasonTypeBySearchBAL getReasonTypeBySearchBAL = new(_Db);
                        objSearchResponse = getReasonTypeBySearchBAL.GetReasonTypeBySearch();
                        return objSearchResponse;
                    });

                    objGetReasonTypeList = await tskResponse;
                    objResult = objGetReasonTypeList ?? objResponse;
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

            objContentResult = new ContentResult() { Content = JsonConvert.SerializeObject(objResult), ContentType = "application/json", StatusCode = StatusCode };
            return objContentResult;
        }
    }

}
