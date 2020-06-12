using DataAccess.Enums;
using Localization;

namespace Logic.Base
{
    public class BaseResponse<TData>
    {
        public BaseResponse()
        {
            Success = false;
            Message = null;
            Data = default;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }

        private ResponseStatusCodes StatusCode;
        private bool IsStatusCodeOverriden { get; set; }

        public BaseResponse<TData> WithStatusCode(ResponseStatusCodes code)
        {
            StatusCode = code;

            IsStatusCodeOverriden = true;

            return this;
        }

        public ResponseStatusCodes ResolveStatusCode()
        {
            if (IsStatusCodeOverriden)
                return StatusCode;

            if (Success)
                return ResponseStatusCodes.Ok;

            return ResponseStatusCodes.BadRequest;
        }

        public static BaseResponse<TData> GetResponseFailed(string message)
        => new BaseResponse<TData>
        {
            Success = false,
            Message = message
        };

        public static BaseResponse<TData> GetResponseSuccess(TData data = default, string message = null)
        => new BaseResponse<TData>
        {
            Success = true,
            Message = message,
            Data = data,
        };

        public static BaseResponse<TData> GetGenericResponseFailed() => GetResponseFailed(Resources.Errors_GenericError);

        public static BaseResponse<TData> GetGenericResponseSuccess(TData data) => GetResponseSuccess(data, Resources.GenericSuccess);

        public static BaseResponse<TData> GetResponseSuccess(TData data) => GetResponseSuccess(data);
    }
}