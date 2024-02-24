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
    public class InsertReasonBAL
    {
        private readonly IOptions<AppSetting> _configurationSettings;
        private readonly General _objGeneral;

        public AppDb _db { get; set; }
        public InsertReasonBAL(AppDb appDb, IOptions<AppSetting> configurationSettings, General objGeneral)
        {
            _db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }
        //***********************************************************************************************************************
        /*
         * Author Name            :  Md Mujahed Ul Islam 
         * Create Date            :  10 Nov 2023
         * Modified Date          : 
         * Modified Reason        : 
         * Layer                  :  BAL
         * Modified By            : 
         * Description            :  This class have the Reason Module Business Logic Code for insert data.
         */
        //***********************************************************************************************************************
        public Response<object> InsertReason(dynamic objReasonInsert)
        {
            _objGeneral.CreateLog("InsertBAL", "InsertReason", "Step 2.1 :Request come to InserReason BAL");
            #region Common Variable
            DateTime startResponseTime = DateTime.Now;
            int ErrorCode = 0;
            string strResponse = string.Empty;
            #endregion

            #region Specific Variables
            Response<object>? objResponse = null;
            string strReasonName = string.Empty;
            int intReasonCode = 0;
            string strId = string.Empty;
            dynamic objValidatedRequest = null;
            int intThirdPartyNumber = 0;
            string strDescription = string.Empty;
            DateTime DateCreated = DateTime.Now;
            Byte IsPublished = Byte.MinValue;
            Byte PrimaryReason = Byte.MinValue;
            string strCreatedBy = string.Empty;
            AppSetting? objRegularExpression = null;
            InsertReasonDAL objInsertReasonDAL = null;
            string? ReasonDetails = null;
            #endregion

            try
            {
                objRegularExpression = new AppSetting();


                #region validation for ReasonName
                if (objReasonInsert != null)
                {
                    if (ErrorCode == 0)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objReasonInsert.ReasonName)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Reason_Name_Required);

                        }
                        else if (Regex.IsMatch(Convert.ToString(objReasonInsert.ReasonName).Trim(), objRegularExpression.RegExSearchWords))
                        {
                            strReasonName = objReasonInsert.ReasonName;

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

                        if (String.IsNullOrEmpty(Convert.ToString(objReasonInsert.ReasonCode)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Reason_Code_is_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objReasonInsert.ReasonCode).Trim(), objRegularExpression.RegExNums))
                        {
                            intReasonCode = objReasonInsert.ReasonCode;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_ReasonCode);

                        }


                    }
                    #endregion

                    #region Validation for  ReasonType.Id


                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objReasonInsert.Id)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.ReasonType_Is_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objReasonInsert.Id).Trim(), objRegularExpression.RegExForId))
                        {
                            strId = objReasonInsert.Id;

                            ///Get by GUID reason type ReasontypeId

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

                        if (String.IsNullOrEmpty(Convert.ToString(objReasonInsert.Description)))
                        {
                            objReasonInsert.Description = null;
                        }
                        else if (Regex.IsMatch(Convert.ToString(objReasonInsert.Description).Trim(), objRegularExpression.RegExSearchWord))
                        {
                            strDescription = objReasonInsert.Description;

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

                        if (String.IsNullOrEmpty(Convert.ToString(objReasonInsert.ThirdPartyNumber)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.ThirdPartyNumber_Is_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objReasonInsert.ThirdPartyNumber).Trim(), objRegularExpression.RegExNum))
                        {
                            intThirdPartyNumber = objReasonInsert.ThirdPartyNumber;
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
                        if (objReasonInsert.IsPublished == 0 || objReasonInsert.IsPublished == 1)
                        {
                            IsPublished = objReasonInsert.IsPublished;
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
                        if (objReasonInsert.PrimaryReason == 0 || objReasonInsert.PrimaryReason == 1)
                        {
                            PrimaryReason = objReasonInsert.PrimaryReason;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_PrimaryReason);
                        }
                    }
                    #endregion

                    #region Validation for CreatedBy
                    if (ErrorCode == 0)
                    {

                        if (String.IsNullOrEmpty(Convert.ToString(objReasonInsert.CreatedBy)))
                        {
                            objReasonInsert.CreatedBy = null;
                        }
                        else if (Regex.IsMatch(Convert.ToString(objReasonInsert.CreatedBy).Trim(), objRegularExpression.RegExSearchWords))
                        {
                            strCreatedBy = objReasonInsert.CreatedBy;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_CreatedBy);
                        }


                    }
                }
                #endregion

                if (ErrorCode == 0)
                {


                    try
                    {
                        objValidatedRequest = new ExpandoObject();
                        objValidatedRequest.strReasonName = strReasonName;
                        objValidatedRequest.intReasonCode = intReasonCode;
                        objValidatedRequest.strId = strId;
                        objValidatedRequest.intThirdPartyNumber = intThirdPartyNumber;
                        objValidatedRequest.strDescription = strDescription;
                        objValidatedRequest.IsPublished = IsPublished;
                        objValidatedRequest.PrimaryReason = PrimaryReason;
                        objValidatedRequest.strCreatedBy = strCreatedBy;
                        objValidatedRequest.DateCreated= DateCreated;

                        if(objValidatedRequest != null)
                        {
                            objInsertReasonDAL = new InsertReasonDAL(_db);
                           _objGeneral.CreateLog("InsertBAL", "InserReason", "Step 2.3 :Request InsertReasonDb in BAL");
                            ReasonDetails = Convert.ToString(objInsertReasonDAL.InsertReasonDb(objValidatedRequest));
                           _objGeneral.CreateLog("InsertBAL", "InsertReason", "Step 2.3 :Response InsertReasonDb in BAL");
                        }
                        if (ErrorCode == 0 && ReasonDetails != null)
                        {
                            strResponse = JsonConvert.SerializeObject(ReasonDetails, Formatting.Indented);
                        }


                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
                        ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);
                    }
                    finally
                    {
                        objReasonInsert = null;
                        objInsertReasonDAL = null;
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
                    else if (ErrorCode == 0 && ReasonDetails != null)
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
