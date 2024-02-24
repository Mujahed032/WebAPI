using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace Revalsys.BusinessLogic
{
    public class GetReasonByIdBAL
    {
        internal AppDb _db;
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        public GetReasonByIdBAL(AppDb appDb, IOptions<AppSetting> configurationSettings, General objGeneral)
        {
            _db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }

        /*
         * Author Name            :  Md Mujahed Ul Islam
         * Create Date            :  17 Nov 2023
         * Modified Date          : 
         * Modified Reason        : 
         * Layer                  :  BAL
         * Modified By            : 
         * Description            :  This class have the Courier Module Business Logic Code.
         */

        public Response<Object> GetReasonById(dynamic objReasonList)
        {
            _objGeneral.CreateLog("GetByIdBAL", "GetReasonById", "Step 2.1 :Request received in GetReasonDetailsById BAL");
            #region Common Variable
            DateTime startResponseTime = DateTime.Now;
            int ErrorCode = 0;
            string strResponse = string.Empty;
            #endregion

            #region Specific Variables
            Response<object> objResponse = null;
            string strid = string.Empty;
            GetReasonByIdDAL objGetReasonByIdDAL = null;
            AppSetting objRegularExpression = null;
            List<ReasonSearchResponseListDTO> reasonDetails = null;
            #endregion

            try
            {
                objRegularExpression = new AppSetting();


                #region Id Validation
                if (objReasonList != null)
                {
                    if (ErrorCode == 0)
                    {
                        if (objReasonList != null)
                        {
                            if (String.IsNullOrWhiteSpace(Convert.ToString(objReasonList.Id)))
                            {
                                ErrorCode = Convert.ToInt32(General.ErrorCode.Id_is_Required);
                            }
                            else if (Regex.IsMatch(Convert.ToString(objReasonList.Id).Trim(), objRegularExpression.RegExForId))
                            {
                                strid = objReasonList.Id;
                            }
                            else
                            {
                                ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Id);

                            }

                        }
                    }

                }
                #endregion

                if (ErrorCode == 0)
                {
                    try
                    {
                        dynamic objValidateRequest = new ExpandoObject();
                        objValidateRequest.Id = strid;

                        if (objValidateRequest != null)
                        {
                            objGetReasonByIdDAL = new GetReasonByIdDAL(_db);
                            _objGeneral.CreateLog("GetByIdBAL", "GetReasonById", "Step 2.2 :Request GetReasonByIdDAL in BAL");
                            reasonDetails = objGetReasonByIdDAL.GetReasonByIdDb(objValidateRequest);
                            _objGeneral.CreateLog("GetByIdBAL", "GetReasonById", "Step 2.3 :Response GetReasonByIdDAL in BAL");
                        }
                        if (reasonDetails != null && reasonDetails.Count > 0)
                        {
                            strResponse = JsonConvert.SerializeObject(reasonDetails, Formatting.Indented);
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.No_Record_Found);
                        }
                    }
                    catch
                    {
                        ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);
                    }
                    finally
                    {
                        objReasonList = null;
                    }
                }
            }
            catch
            {
                ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);
            }
            finally
            {
                try
                {

                    if (ErrorCode > 0)
                    {
                        ErrorCodeDAL errorCodeDAL = new(_db);
                        objResponse = new Response<object>();
                        objResponse.ReturnCode = ErrorCode;
                        _objGeneral.CreateLog("GetByIdBAL", "GetReasonById", "Step 2.4 :Request GetErrorCode in BAL");
                        objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
                        _objGeneral.CreateLog("GetByIdBAL", "GetReasonById", "Step 2.5 :Response GetErrorCode in BAL");
                        objResponse.Data = null;
                    }
                    else if (ErrorCode == 0 && reasonDetails != null)
                    {
                        objResponse = new Response<object>();
                        objResponse.ReturnCode = ErrorCode;
                        objResponse.ReturnMessage = "Success";
                        var json = JsonConvert.DeserializeObject<dynamic>(strResponse);
                        objResponse.Data = json;

                    }
                }
                catch (Exception)
                {
                    ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);

                }
                finally
                {
                    objRegularExpression = null;
                }
            }
            return objResponse;
        }
    }
}
