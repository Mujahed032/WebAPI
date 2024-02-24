using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using Serilog;
using System;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace Revalsys.BusinessLogic
{
    public class DeleteReasonBAL
    {
        internal AppDb _db;
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        public DeleteReasonBAL(AppDb appDb, IOptions<AppSetting> configurationSettings, General objGeneral)
        {
            _db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }

        /*
         * Author Name            :  Md Mujahed Ul Islam 
         * Create Date            :  18 Nov 2023
         * Modified Date          : 
         * Modified Reason        : 
         * Layer                  :  BAL
         * Modified By            : 
         * Description            :  This class have the Reason Module Business Logic Code for delete the Reason.
         */
        public Response<object> DeleteReason(dynamic objDeleteReason)
        {
            _objGeneral.CreateLog("DeleteByIdBAL", "DeleteReason", "Step 2.1 :Request received in DeleteReasonBAL");
            #region Common Variable
            DateTime startResponseTime = DateTime.Now;
            int ErrorCode = 0;
            string strResponse = string.Empty;
            #endregion

            #region Specific Variables
            Response<object> objResponse = null;
            int? intRevalReasonId = null;
            string strId = string.Empty;
            CommonDAL objCommonDAL = null;
            string strDeletedBy = string.Empty;
            DateTime strDateDeleted = DateTime.Now;
            DeleteReasonDAL objDeleteReasonDAL = null;
            AppSetting objRegularExpression = null;
            string? deleteDetails = null;
            #endregion

            try
            {
                #region Id Validation
                objRegularExpression = new AppSetting();

                if (objDeleteReason != null)
                {
                    if (ErrorCode == 0)
                    {
                        if (String.IsNullOrWhiteSpace(Convert.ToString(objDeleteReason.Id)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Id_is_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objDeleteReason.Id).Trim(), objRegularExpression.RegExForId))
                        {
                            strId = objDeleteReason.Id;

                            objCommonDAL = new CommonDAL(_db);
                            intRevalReasonId = objCommonDAL.GetReasonPrimaryId(strId);
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Id);

                        }

                    }

                    #endregion

                    #region Validation for DeletedBy
                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objDeleteReason.DeletedBy)))
                        {
                            objDeleteReason.DeletedBy = null;
                        }
                        else if (Regex.IsMatch(Convert.ToString(objDeleteReason.DeletedBy).Trim(), objRegularExpression.RegExSearchWord))
                        {
                            strDeletedBy = objDeleteReason.DeletedBy;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_DeletedBy);
                        }
                    }

                }

                #endregion

                if (ErrorCode == 0)
                {
                    try
                    {
                        dynamic objValidatedRequest = new ExpandoObject();
                        objValidatedRequest.strId = strId;
                        objValidatedRequest.DeletedBy = strDeletedBy;
                        objValidatedRequest.DateDeleted = strDateDeleted;
                        objValidatedRequest.RevalReasonId = intRevalReasonId;
                        if (objValidatedRequest != null)
                        {
                            objDeleteReasonDAL = new DeleteReasonDAL(_db);
                            _objGeneral.CreateLog("DeleteByIdBAL", "DeleteReason", "Step 2.2 :Request DeleteReasonDAL in BAl");
                            deleteDetails = objDeleteReasonDAL.DeleteReasonDb(objValidatedRequest);
                            _objGeneral.CreateLog("DeleteByIdBAL", "DeleteReason", "Step 2.3 :Response DeleteReasonDAL in BAL");
                        }
                        if (deleteDetails != null)
                        {
                            strResponse = JsonConvert.SerializeObject(deleteDetails, Formatting.Indented);
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
                        objDeleteReason = null;
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
                        _objGeneral.CreateLog("DeleteByIdBAL", "DeleteReason", "Step 2.4 :Request GetErrorCode in BAL");
                        objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
                        _objGeneral.CreateLog("DeleteByIdBAL", "DeleteReason", "Step 2.5 :Response GetErrorCode in BAL");
                        objResponse.Data = null;
                    }
                    else if (ErrorCode == 0 && deleteDetails != null)
                    {
                        objResponse = new Response<object>();
                        objResponse.ReturnCode = ErrorCode;
                        objResponse.ReturnMessage = "Success";
                        var json = JsonConvert.DeserializeObject<dynamic>(strResponse);
                        objResponse.Data = json;

                    }
                }
                catch
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
