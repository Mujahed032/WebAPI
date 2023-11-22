using Revalsys.DataAccess;

namespace Revalsys.Utilities
{
    public class General
    {
        public IHttpClientFactory ClientFactory { get; }
        internal AppDb Db { get; set; }

        public General(IHttpClientFactory clientFactory, AppDb db)
        {
            ClientFactory = clientFactory;
            Db = db;
        }

        public enum ErrorCode
        {
            Success = 0,
            Technical_Error_Occured = 1,
            No_Record_Found = 2,
            Reason_Name_Required = 3,
            Reason_Code_is_Required = 4, 
            Invalid_Id = 5,
            ThirdPartyNumber_Is_Required = 6,
            Id_is_Required = 7,
            Invalid_Reason_Type = 8,
            Invalid_Reason_Name = 9,
            ReasonName_Already_Exist = 10,
            DeletedBy_is_Required = 11,
            Invalid_IsPublished = 12,
            Invalid_Description = 13,
            Invalid_PageSize = 14,
            Invalid_PageNumber = 15,
            Invalid_ReasonCode = 16,
            ReasonType_Is_Required = 17,
            Invalid_ThirdPartyNumber = 18,
            Invalid_PrimaryReason = 19,
            Invalid_CreatedBy = 20,
            Invalid_UpdatedBy = 21,
            Invalid_DeletedBy = 22
        }

        #region CommonResponseErrorCodes
        public enum CommonResponseErrorCodes
        {
            //http status codes
            Success = 200,
            RequestTimeOut = 408,
            BadRequest = 400,
            UnAuthorized = 401,
            Forbidden = 403,
            NotAcceptable = 406,
            InvalidRequest = -101,
            No_Records_Found = 22
        }

        public static Dictionary<string, string> dictCommonResponse
        {
            get
            {
                Dictionary<string, string> dictCommonResponse = new Dictionary<string, string>();
                dictCommonResponse.Add(CommonResponseErrorCodes.Success.ToString(), "Success");
                dictCommonResponse.Add(CommonResponseErrorCodes.RequestTimeOut.ToString(), "Request Time Out");
                dictCommonResponse.Add(CommonResponseErrorCodes.BadRequest.ToString(), "Bad Request");
                dictCommonResponse.Add(CommonResponseErrorCodes.UnAuthorized.ToString(), "UnAuthorized");
                dictCommonResponse.Add(CommonResponseErrorCodes.Forbidden.ToString(), "Forbidden");
                dictCommonResponse.Add(CommonResponseErrorCodes.NotAcceptable.ToString(), "Not Acceptable");
                dictCommonResponse.Add(CommonResponseErrorCodes.InvalidRequest.ToString(), "Invalid Request");
                dictCommonResponse.Add(CommonResponseErrorCodes.No_Records_Found.ToString(), "No Records Found");

                return dictCommonResponse;
            }
        }
        #endregion
    }
}