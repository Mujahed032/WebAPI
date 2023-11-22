using Newtonsoft.Json;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace Revalsys.BusinessLogic
{
    public class DeleteReasonBAL
    {
        internal AppDb _db;
        public DeleteReasonBAL(AppDb appDb)
        {
            _db = appDb;
        }

        /*
         * Author Name            :  Md Mujahed Ul Islam 
         * Create Date            :  18 Nov 2023
         * Modified Date          : 
         * Modified Reason        : 
         * Layer                  :  BAL
         * Modified By            : 
         * Description            :  This class have the Reason Module Business Logic Code.
         */
        public Response<object> DeleteReason(dynamic objDeleteReason)
        {
            #region Common Variable
            DateTime startResponseTime = DateTime.Now;
            int ErrorCode = 0;
            string strResponse = string.Empty;
            #endregion

            #region Specific Variables
            Response<object> objResponse = null;
            string strid = string.Empty;
            string strDeletedBy = string.Empty;
            DateTime strDateDeleted = DateTime.Now;
            DeleteReasonDAL objDeleteReasonDAL = null;
            RegularExpression objRegularExpression = null;
            string? deleteDetails = null;
            #endregion

            try
            {
                #region Id Validation
                objRegularExpression = new RegularExpression();
                if (ErrorCode == 0)
                {
                    if (ErrorCode == 0)
                    {
                        if (objDeleteReason != null)
                        {
                            if (String.IsNullOrWhiteSpace(Convert.ToString(objDeleteReason.Id)))
                            {
                                ErrorCode = Convert.ToInt32(General.ErrorCode.Id_is_Required);
                            }
                            else if (Regex.IsMatch(Convert.ToString(objDeleteReason.Id).Trim(), objRegularExpression.RegExForId))
                            {
                                strid = objDeleteReason.Id;
                            }
                            else
                            {
                                ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Id);

                            }

                        }
                    }
                    #endregion

                    #region Validation for DeletedBy
                    if (ErrorCode == 0)
                    {
                        if (objDeleteReason != null)
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
                            objValidatedRequest.Id = strid;
                            objValidatedRequest.DeletedBy = strDeletedBy;
                            objValidatedRequest.DateDeleted = strDateDeleted;
                            if (objValidatedRequest != null)
                            {
                                objDeleteReasonDAL = new DeleteReasonDAL(_db);
                                deleteDetails = objDeleteReasonDAL.DeleteReasonDb(objValidatedRequest);
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
