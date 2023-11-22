using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Revalsys.BusinessLogic
{
    public class InsertReasonBAL
    {
        public AppDb _db { get; set; }
        public InsertReasonBAL(AppDb appDb)
        {
            _db = appDb;
        }

        /*
         * Author Name            :  Md Mujahed Ul Islam 
         * Create Date            :  09 Nov 2023
         * Modified Date          : 
         * Modified Reason        : 
         * Layer                  :  BAL
         * Modified By            : 
         * Description            :  This class have the Reason Module Business Logic Code.
         */
        public Response<object> InsertReason(dynamic objReasonInsert)
        {
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
            DateTime DateCreated = DateTime.MinValue;
            Byte IsPublished = Byte.MinValue;
            Byte PrimaryReason = Byte.MinValue;
            string strCreatedBy = string.Empty;
            RegularExpression? objRegularExpression = null;
            InsertReasonDAL objInsertReasonDAL = null;
            string? ReasonDetails = null;
            #endregion

            try
            {
                objRegularExpression = new RegularExpression();


                #region validation for ReasonName
                if (ErrorCode == 0)
                {
                    if (objReasonInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objReasonInsert.ReasonName)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Reason_Name_Required);

                        }
                        else if (Regex.IsMatch(Convert.ToString(objReasonInsert.ReasonName).Trim(), objRegularExpression.RegExSearchWord))
                        {
                            strReasonName = objReasonInsert.ReasonName;
                            
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Reason_Name);
                        }

                    }
                   
                }
                #endregion

                #region Validation for ReasonCode

                if (ErrorCode == 0)
                {
                    if (objReasonInsert != null)
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
                   
                }
                #endregion

                #region Validation for  ReasonType.Id


                if (ErrorCode == 0)
                {
                    if (objReasonInsert != null)
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
                   
                }

                #endregion

                #region Validation for Description
                if (ErrorCode == 0)
                {
                    if (objReasonInsert != null)
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
                    
                }

                #endregion

                #region Validation for ThirdPartyNumber

                if (ErrorCode == 0)
                {
                    if (objReasonInsert != null)
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
                    if (objReasonInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objReasonInsert.CreatedBy)))
                        {
                            objReasonInsert.CreatedBy = null;
                        }
                        else if (Regex.IsMatch(Convert.ToString(objReasonInsert.CreatedBy).Trim(), objRegularExpression.RegExSearchWord))
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
                        objValidatedRequest.DateCreated= DateTime.Now;

                        if(objInsertReasonDAL != null)
                        {
                            objInsertReasonDAL = new InsertReasonDAL(_db);
                            ReasonDetails = objInsertReasonDAL.InsertReasonDb(objValidatedRequest);
                        }
                        if (ErrorCode == 0 && ReasonDetails != null)
                        {
                            strResponse = JsonConvert.SerializeObject(ReasonDetails, Formatting.Indented);
                        }


                    }
                    catch
                    {
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
                        objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
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
