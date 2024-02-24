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
    public class InsertColorBAL
    {
        private readonly IOptions<Appsetting> _configurationSettings;
        private readonly General _objGeneral;

        public AppDb _db { get; set; }
        public InsertColorBAL(AppDb appDb, IOptions<Appsetting> configurationSettings, General objGeneral)
        {
            _db = appDb;
            _configurationSettings = configurationSettings;
            _objGeneral = objGeneral;
        }

        /*
         * Author Name            :  Md Mujahed Ul Islam 
         * Create Date            :  23 Nov 2023
         * Modified Date          : 
         * Modified Reason        : 
         * Layer                  :  BAL
         * Modified By            : 
         * Description            :  This class have the Color Module Business Logic Code to insert the Data.
         */
        public Response<object> InsertColor(dynamic objColorInsert)
        {
            _objGeneral.CreateLog("InsertBAL", "InsertColor", "Step 2.1 :Request come to InserColor BAL");
            #region Common Variable
            DateTime startResponseTime = DateTime.Now;
            int ErrorCode = 0;
            string strResponse = string.Empty;
            #endregion

            #region Specific Variables
            Response<object>? objResponse = null;
            string strColorFamily = string.Empty;
            string strColorValue = string.Empty;
            string CstrId = string.Empty;
            string IstrId = string.Empty;
            string SstrId = string.Empty;
            dynamic objValidatedRequest = null;
            string strDescription = string.Empty;
            DateTime DateCreated = DateTime.Now;
            Byte IsPublished = Byte.MinValue;
            string strCreatedBy = string.Empty;
            Appsetting? objRegularExpression = null;
            InsertColorDAL objInsertColorDAL = null;
            string? ColorDetails = null;
            string Id = string.Empty;
            var json = default(string);
            #endregion

            try
            {
                objRegularExpression = new Appsetting();


                #region validation for ColorFamily
                if (ErrorCode == 0)
                {
                    if (objColorInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objColorInsert.ColorFamily)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Color_Family_Required);

                        }
                        else if (Regex.IsMatch(Convert.ToString(objColorInsert.ColorFamily).Trim(), objRegularExpression.RegExSearchWords))
                        {
                            strColorFamily = objColorInsert.ColorFamily;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General. ErrorCode.Invalid_Color_Family);
                        }

                    }

                }
                #endregion

                #region Validation for ColorValue

                if (ErrorCode == 0)
                {
                    if (objColorInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objColorInsert.ColorValue)))
                        {
                            objColorInsert.ColorValue = null;//error massage
                        }
                        else if (Regex.IsMatch(Convert.ToString(objColorInsert.ColorValue).Trim(), objRegularExpression.RegExNum))
                        {
                            strColorValue = objColorInsert.ColorValue;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_ColorValue);

                        }
                    }

                }
                #endregion

                #region Validation for Description
                if (ErrorCode == 0)
                {
                    if (objColorInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objColorInsert.Description)))
                        {
                            objColorInsert.Description = null;//error massage
                        }
                        else if (Regex.IsMatch(Convert.ToString(objColorInsert.Description).Trim(), objRegularExpression.RegExSearchWord))
                        {
                            strDescription = objColorInsert.Description;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Description);
                        }
                    }

                }

                #endregion


                #region IsPublished Validations 
                if (ErrorCode == 0)
                {
                    if (objColorInsert.IsPublished == 0 || objColorInsert.IsPublished == 1)
                    {
                        IsPublished = objColorInsert.IsPublished;
                    }
                    else
                    {
                        ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_IsPublished);
                    }
                }
                #endregion


                #region Validation for CreatedBy
                if (ErrorCode == 0)
                {
                    if (objColorInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objColorInsert.CreatedBy)))
                        {
                            objColorInsert.CreatedBy = null;
                        }
                        else if (Regex.IsMatch(Convert.ToString(objColorInsert.CreatedBy).Trim(), objRegularExpression.RegExSearchWords))
                        {
                            strCreatedBy = objColorInsert.CreatedBy;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_CreatedBy);
                        }
                    }

                }

                #endregion

                #region Validation for ColorCode.ColorCodeId
                if (ErrorCode == 0)
                {
                    if (objColorInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objColorInsert.ColorCodeId)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Color_Code_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objColorInsert.ColorCodeId).Trim(), objRegularExpression.RegExForId))
                        {
                            CstrId = objColorInsert.ColorCodeId;
                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Color_Code);
                        }
                    }
                }
                #endregion



                #region Validation for  ColorImage.ColorImageId


                if (ErrorCode == 0)
                {
                    if (objColorInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objColorInsert.ColorImageId)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Color_Image_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objColorInsert.ColorImageId).Trim(), objRegularExpression.RegExForId))
                        {
                            IstrId = objColorInsert.ColorImageId;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Color_Image);
                        }
                    }

                }

                #endregion

                #region Validation for  SwatchColor.SwatchColorId


                if (ErrorCode == 0)
                {
                    if (objColorInsert != null)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(objColorInsert.SwatchColorId)))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Swatch_Color_Required);
                        }
                        else if (Regex.IsMatch(Convert.ToString(objColorInsert.SwatchColorId).Trim(), objRegularExpression.RegExForId))
                        {
                            SstrId = objColorInsert.SwatchColorId;

                        }
                        else
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Invalid_Swatch_Color);
                        }
                    }

                }

                #endregion

                if (ErrorCode == 0)
                {


                    try
                    {
                        objValidatedRequest = new ExpandoObject();
                        objValidatedRequest.strColorFamily = strColorFamily;
                        objValidatedRequest.strColorValue = strColorValue;
                        objValidatedRequest.CstrId = CstrId;
                        objValidatedRequest.IstrId = IstrId;
                        objValidatedRequest.SstrId = SstrId;
                        objValidatedRequest.strDescription = strDescription;
                        objValidatedRequest.IsPublished = IsPublished;
                        objValidatedRequest.strCreatedBy = strCreatedBy;
                        objValidatedRequest.DateCreated = DateCreated;

                        if (objValidatedRequest != null)
                        {
                            objInsertColorDAL = new InsertColorDAL(_db);
                            _objGeneral.CreateLog("InsertBAL", "InserColor", "Step 2.3 :Request InsertColorDb in BAL");
                            ColorDetails = Convert.ToString(objInsertColorDAL.InsertColorDb(objValidatedRequest));
                            _objGeneral.CreateLog("InsertBAL", "InserColor", "Step 2.3 :Response InsertColorDb in BAL");
                        }
                        Console.WriteLine($"ColorDetails Type: {ColorDetails?.GetType()?.FullName}");
                        if (ErrorCode == 0 && ColorDetails != null  )
                        {
                            Id = ColorDetails;
                        }
                        json = JsonConvert.SerializeObject(Id);

                    }

                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("UQ_tblRevalColor_ColorValue"))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.ColorValue_Unique);
                        }
                        else if (ex.Message.Contains("UQ_tblRevalColor_ColorFamily_ColorCodeId"))
                        {
                            ErrorCode = Convert.ToInt32(General.ErrorCode.ColorFamily_Unique);
                        }
                        else
                        {
                            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
                            ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);
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
                        _objGeneral.CreateLog("UpdateBAL", "UpdateColor", "Step 2.4 :Request GetErrorCode in BAL");
                        objResponse.ReturnMessage = errorCodeDAL.GetErrorCode(ErrorCode);
                        _objGeneral.CreateLog("UpdateBAL", "UpdateColor", "Step 2.5 :Response GetErrorCode in BAL");
                        objResponse.Data = null;
                    }
                    else if (ErrorCode == 0 && Id != null)
                    {
                        objResponse = new Response<object>();
                        objResponse.ReturnCode = ErrorCode;
                        objResponse.ReturnMessage = "Success";
                        objResponse.Data = new { Id = JsonConvert.DeserializeObject(json) };

                    }
                }
                catch
                {
                    ErrorCode = Convert.ToInt32(General.ErrorCode.Technical_Error_Occured);

                }
                
            }
            return objResponse;

        }
    }

}