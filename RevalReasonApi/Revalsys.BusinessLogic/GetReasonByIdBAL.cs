using Newtonsoft.Json;
using Revalsys.Common;
using Revalsys.DataAccess;
using Revalsys.Properties;
using Revalsys.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Revalsys.BusinessLogic
{
    public class GetReasonByIdBAL
    {
        internal AppDb _db;
        public GetReasonByIdBAL(AppDb appDb)
        {
            _db = appDb;
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
            #region Common Variable
            DateTime startResponseTime = DateTime.Now;
            int ErrorCode = 0;
            string strResponse = string.Empty;
            #endregion

            #region Specific Variables
            Response<object> objResponse = null;
            string strid = string.Empty;
            GetReasonByIdDAL objGetReasonByIdDAL = null;
            RegularExpression objRegularExpression = null;
            List<ReasonSearchResponseListDTO> reasonDetails = null;
            #endregion

            try
            {
                objRegularExpression = new RegularExpression();

                if (ErrorCode == 0)
                {
                    #region Id Validation
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
                            reasonDetails = objGetReasonByIdDAL.GetReasonByIdDb(objValidateRequest);
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
                        objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
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
