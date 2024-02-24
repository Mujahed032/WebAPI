using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using Serilog;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace Revalsys.BusinessLogic
{
    public class UpdateReasonBAL
    {
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;
        public AppDb _db { get; set; }
        public UpdateReasonBAL(AppDb appDb,IOptions<AppSetting> configurationSettings, General objGeneral)
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
         * Description            :  This class have the Reason Module Business Logic Code for update data.
         */
        public Response<object> UpdateReason(dynamic objUpdateReason)
        {
            _objGeneral.CreateLog("UpadteBAL", "UpdateReason", "Step 2.1 :Request come to UpdateReasonBAL");
            #region Common Variable
            DateTime startResponseTime = DateTime.Now;
            int ErrorCode = 0;
            string strResponse = string.Empty;
            #endregion

            #region Specific Variables
            Response<object>? objResponse = null;
            string strReasonName = string.Empty;
            int intReasonCode = 0;
            int? intReasonId = null;
            string strRId = string.Empty;
            string strId = string.Empty;
            int intReasonTypeId = 0;
            int intThirdPartyNumber = 0;
            string strDescription = string.Empty;
            DateTime LastUpdated = DateTime.Now;
            Byte IsPublished = Byte.MinValue;
            Byte PrimaryReason = Byte.MinValue;
            string strUpdatedBy = string.Empty;
            AppSetting? objRegularExpression = null;
            UpdateReasonDAL objUpdateReasonDAL = null;
            ReasonTypeDAL objReasonTypeDAL = null;
            CommonDAL objCommonDAL = null;
            string? updateDetails = null;

            #endregion
            strId = objUpdateReason.Id;

            try
            {
                objRegularExpression = new AppSetting();

                #region Validation for  Id

                if (objUpdateReason != null)
                {
                    if (ErrorCode == 0)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objUpdateReason.Id)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.ReasonType_Is_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objUpdateReason.Id).Trim(), objRegularExpression.RegExForId))
                        {
                            strId = objUpdateReason.Id;

                            objCommonDAL = new CommonDAL(_db);
                            intReasonId = objCommonDAL.GetReasonPrimaryId(strId);

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Id);
                        }
                    }



                    #endregion


                    #region validation for ReasonName
                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objUpdateReason.ReasonName)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Reason_Name_Required);

                        }
                        else if (Regex.IsMatch(Convert.ToString(objUpdateReason.ReasonName).Trim(), objRegularExpression.RegExSearchWords))
                        {
                            strReasonName = objUpdateReason.ReasonName;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Reason_Name);
                        }



                    }
                    #endregion

                    #region Validation for ReasonCode

                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objUpdateReason.ReasonCode)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Reason_Code_is_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objUpdateReason.ReasonCode).Trim(), objRegularExpression.RegExNum))
                        {
                            intReasonCode = objUpdateReason.ReasonCode;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_ReasonCode);

                        }


                    }
                    #endregion

                    #region Validation for  ReasonType.RId


                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objUpdateReason.RId)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.ReasonType_Is_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objUpdateReason.RId).Trim(), objRegularExpression.RegExForId))
                        {
                            
                            objReasonTypeDAL = new ReasonTypeDAL(_db);
                            intReasonTypeId = objReasonTypeDAL.GetReasonTypeId(strRId);

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Reason_Type);
                        }


                    }

                    #endregion

                    #region Validation for Description
                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objUpdateReason.Description)))
                        {
                            objUpdateReason.Description = null;
                        }
                        else if (Regex.IsMatch(Convert.ToString(objUpdateReason.Description).Trim(), objRegularExpression.RegExSearchWord))
                        {
                            strDescription = objUpdateReason.Description;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Description);
                        }


                    }

                    #endregion

                    #region Validation for ThirdPartyNumber

                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objUpdateReason.ThirdPartyNumber)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.ThirdPartyNumber_Is_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objUpdateReason.ThirdPartyNumber).Trim(), objRegularExpression.RegExNum))
                        {
                            intThirdPartyNumber = objUpdateReason.ThirdPartyNumber;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_ThirdPartyNumber);

                        }


                    }
                    #endregion

                    #region IsPublished Validations 
                    if (ErrorCode == 0)
                    {
                        if (objUpdateReason.IsPublished == 0 || objUpdateReason.IsPublished == 1)
                        {
                            IsPublished = objUpdateReason.IsPublished;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_IsPublished);
                        }
                    }
                    #endregion

                    #region PrimaryReason Validations 
                    if (ErrorCode == 0)
                    {
                        if (objUpdateReason.PrimaryReason == 0 || objUpdateReason.PrimaryReason == 1)
                        {
                            PrimaryReason = objUpdateReason.PrimaryReason;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_PrimaryReason);
                        }
                    }
                    #endregion

                    #region Validation for UpdatedBy
                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objUpdateReason.UpdatedBy)))
                        {
                            objUpdateReason.UpdatedBy = null;
                        }
                        else if (Regex.IsMatch(Convert.ToString(objUpdateReason.UpdatedBy).Trim(), objRegularExpression.RegExSearchWords))
                        {
                            strUpdatedBy = objUpdateReason.UpdatedBy;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_UpdatedBy);
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
                        objValidatedRequest.strReasonName = strReasonName;
                        objValidatedRequest.strDescription = strDescription;
                        objValidatedRequest.intThirdPartyNumber = intThirdPartyNumber;
                        objValidatedRequest.intReasonCode = intReasonCode;
                        objValidatedRequest.intReasonTypeId = intReasonTypeId;
                        objValidatedRequest.IsPublished = IsPublished;
                        objValidatedRequest.PrimaryReason = PrimaryReason;
                        objValidatedRequest.strUpdatedBy = strUpdatedBy;
                        objValidatedRequest.LastUpdated = LastUpdated;
                        objValidatedRequest.intReasonId = intReasonId;
                        if (objValidatedRequest != null)
                        {
                            objUpdateReasonDAL = new UpdateReasonDAL(_db);
                            _objGeneral.CreateLog("UpdateBAL", "UpdateReason", "Step 2.3 :Request UpdateReasonDb in BAL");
                            updateDetails = objUpdateReasonDAL.UpdateReasonDb(objValidatedRequest);
                            _objGeneral.CreateLog("UpdateBAL", "UpdateReason", "Step 2.3 :Response UpdateReasonDb in BAL");
                        }
                        if (ErrorCode == 0 && updateDetails != null)
                        {
                            strResponse = JsonConvert.SerializeObject(updateDetails, Formatting.Indented);
                        }


                    }
                    catch
                    {
                        ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);
                    }
                    finally
                    {

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
                        _objGeneral.CreateLog("UpdateBAL", "UpdateReason", "Step 2.4 :Request GetErrorCode in BAL");
                        objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
                        _objGeneral.CreateLog("UpdateBAL", "UpdateReason", "Step 2.5 :Response GetErrorCode in BAL");
                        objResponse.Data = null;
                    }
                    else if (ErrorCode == 0 && updateDetails != null)
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
