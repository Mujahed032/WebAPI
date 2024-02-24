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
    public class GetReasonBySearchBAL
    {
        private AppDb? _Db;
        private General _objGeneral;
        private IOptions<AppSetting> _objConfigurationSetting;
        public GetReasonBySearchBAL(AppDb? Db, General objGeneral, IOptions<AppSetting> objConfigurationSetting)
        {
            _Db = Db;
            _objGeneral = objGeneral;
            _objConfigurationSetting = objConfigurationSetting;
        }
        //***********************************************************************************************************************
        /*
         * Author Name            :  Md Mujahed Ul Islam 
         * Create Date            :  09 Nov 2023
         * Modified Date          : 
         * Modified Reason        : 
         * Layer                  :  BAL
         * Modified By            : 
         * Description            :  This class have the Reason Module Business Logic Code for Get the Reason Details By search.
         */
        //***********************************************************************************************************************
        public Response<object> GetReasonBySearch(dynamic objGetReasonList)
        {
            _objGeneral.CreateLog("GetBySearchBAL", "GetReasonBySearch", "Step 2.1 :Request received in GetReasonBySearch BAL");
            #region Common Variable
            DateTime startResponseTime = DateTime.Now;
            int ErrorCode = 0;
            string strResponse = string.Empty;
            #endregion

            #region Specific Variables
            Response<object> objResponse = null;
            int intPageNumber = 0, intPageSize = 0;
            GetReasonBySearchDAL objGetReasonBySerachDAL = null;
            ErrorCodeDAL errorCodeDAL = null;
            string strreasonName = string.Empty;
            AppSetting? objRegularExpression = null;
            List<ReasonSearchResponseListDTO> reasonDetails = null;
            GetReasonBySearchDAL objGetReasonBySearchDAL = null;
            #endregion

            try
            {
                objRegularExpression = new AppSetting();

                #region SearchWord Validation
                if (objGetReasonList != null)
                {
                    if (ErrorCode == 0)
                    {
                        if (objGetReasonList != null && !String.IsNullOrEmpty(Convert.ToString(objGetReasonList.ReasonName)))
                        {
                            if (Regex.IsMatch(Convert.ToString(objGetReasonList.ReasonName).Trim(), objRegularExpression.RegExSearchWord))
                            {
                                strreasonName = objGetReasonList.ReasonName;

                            }
                            else
                            {
                                ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Reason_Name);
                            }

                        }
                    }
                    #endregion

                    #region  PageNumber Validation
                    if (ErrorCode == 0)
                    {
                        if (objGetReasonList.PageNumber != null && !String.IsNullOrWhiteSpace(Convert.ToString(objGetReasonList.PageNumber)))
                        {
                            if (Regex.IsMatch(Convert.ToString(objGetReasonList.PageNumber).Trim(), objRegularExpression.RegExNum))
                            {
                                intPageNumber = Convert.ToInt32(objGetReasonList.PageNumber);

                            }
                            else
                            {
                                ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_PageNumber);
                            }
                        }
                    }
                    #endregion


                    #region  PageSize Validation
                    if (ErrorCode == 0)
                    {
                        if (objGetReasonList.PageSize != null && !String.IsNullOrWhiteSpace(Convert.ToString(objGetReasonList.PageSize)))
                        {
                            if (Regex.IsMatch(Convert.ToString(objGetReasonList.PageSize).Trim(), objRegularExpression.RegExNum))
                            {
                                intPageSize = Convert.ToInt32(objGetReasonList.Invalid_PageSize);
                            }
                            else
                            {

                                ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_PageSize);
                            }
                        }
                    }
                }
                #endregion

                if (ErrorCode == 0)
                {
                    if (ErrorCode == 0)
                    {
                        if (intPageNumber <= 0 || intPageSize <= 0)
                        {
                            intPageNumber = 1;
                            intPageSize = 999999999;
                        }
                    }

                    if (intPageNumber == 1 && intPageSize == 999999999)
                    {
                        objGetReasonList.PageSize = intPageSize;
                        objGetReasonList.PageNumber = intPageNumber;
                    }

                    try
                    {

                        dynamic objValidatedRequest = new ExpandoObject();
                        objValidatedRequest.ReasonName = strreasonName;
                        objValidatedRequest.PageNumber = intPageNumber;
                        objValidatedRequest.PageSize = intPageSize;
                        if (objValidatedRequest != null)
                        {
                            objGetReasonBySerachDAL = new GetReasonBySearchDAL(_Db);
                            _objGeneral.CreateLog("GetBySearchBAL", "GetReasonBySearch", "Step 2.2 :Request GetReasonBySearchDb in BAL");

                            reasonDetails = objGetReasonBySerachDAL.GetReasonBySearchDb(objValidatedRequest);
                            _objGeneral.CreateLog("GetBySearchBAL", "GetReasonBySearch", "Step 2.3 :Response GetReasonBySearchDb in BAL");
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
                    catch (Exception ex)
                    {

                        ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);
                    }
                    finally
                    {
                        objGetReasonList = null;
                    }

                }

            }
            catch (Exception ex)
            {

                ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);
            }
            finally
            {
                try
                {
                    if (ErrorCode > 0)
                    {
                        errorCodeDAL = new ErrorCodeDAL(_Db);
                        objResponse = new Response<object>();
                        objResponse.ReturnCode = ErrorCode;
                        _objGeneral.CreateLog("GetBySearchBAL", "GetReasonBySearch", "Step 2.4 :Request GetErrorCode in BAL");
                        objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
                        _objGeneral.CreateLog("GetBySearchBAL", "GetReasonBySearch", "Step 2.5 :Response GetErrorCode in BAL");
                        objResponse.Data = null;

                    }
                    else if (ErrorCode == 0 && reasonDetails != null && reasonDetails.Count > 0)
                    {
                        objResponse = new Response<object>();
                        objResponse.ReturnCode = ErrorCode;
                        objResponse.ReturnMessage = "Success";
                        objResponse.RecordCount = reasonDetails.Count;
                        objResponse.ResponseTime = Math.Round((DateTime.Now - startResponseTime).TotalMilliseconds).ToString();
                        var json = JsonConvert.DeserializeObject<dynamic>(strResponse);
                        objResponse.Data = json;

                    }
                    objResponse.ResponseTime = Math.Round((DateTime.Now - startResponseTime).TotalMilliseconds).ToString();
                }
                catch (Exception ex)
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
