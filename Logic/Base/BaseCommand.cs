using System;
using DataAccess.Entities;
using DataAccess.Entities.User;
using DataAccess.Enums;
using Localization;
using Services.Interfaces.Common;
using Services.Interfaces.Project;

namespace Logic.Base
{
    public abstract class BaseCommand<TResponse> : IApiCommand<TResponse> where TResponse : class
    {
        public IExceptionLogger ExceptionLogger { get; set; }
        
        public ApplicationUser CurrentApplicationUser { get; set; }

        public bool ShouldThrowExceptions { get; set; }

        public virtual bool AllowAnonymous => false;

        protected BaseResponse<TResponse> GetResponseFailed(string message)
        {
            return BaseResponse<TResponse>.GetResponseFailed(message);
        }

        protected BaseResponse<TResponse> GetResponseSuccess(TResponse data = default, string message = null)
        {
            return BaseResponse<TResponse>.GetResponseSuccess(data, message);
        }

        protected BaseResponse<TResponse> GetGenericResponseFailed()
        {
            return BaseResponse<TResponse>.GetGenericResponseFailed();
        }

        protected BaseResponse<TResponse> GetGenericResponseSuccess(TResponse data)
        {
            return BaseResponse<TResponse>.GetGenericResponseSuccess(data);
        }

        public (BaseResponse<TResponse> Response, Project Project) ValidateProjectAccess(IProjectService projectService, int projectId)
        {
            var result = projectService.ValidateAccess(CurrentApplicationUser, projectId);

            return ValidateProjectAccess(result.Project, result.ErrorMessage, result.Unauthorized);
        }

        private (BaseResponse<TResponse> Response, Project Project) ValidateProjectAccess(Project project, string errorMessage, bool unauthorized)
        {
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                if (unauthorized)
                    return (GetResponseFailed(errorMessage).WithStatusCode(ResponseStatusCodes.Unauthorized), null);

                return (GetResponseFailed(errorMessage), null);
            }

            return (GetResponseSuccess(), project);
        }

        public BaseResponse<TResponse> Execute()
        {
            if (!AllowAnonymous && CurrentApplicationUser is null)
            {
                return GetResponseFailed(Resources.Errors_AuthenticationFailed);
            }

            try
            {
                return ExecuteCore();
            }
            catch (Exception ex)
            {
                if (ShouldThrowExceptions)
                    throw ex;

                ExceptionLogger.LogException(ex);
                
                return GetGenericResponseFailed();
            }
        }

        public int ParseIntParam(string param)
        {
            var isNumber = int.TryParse(param, out int number);

            if (!isNumber)
                throw new InvalidCastException();

            return number;
        }

        protected virtual BaseResponse<TResponse> ExecuteCore() => GetGenericResponseFailed();
    }

    public abstract class BaseCommand<TRequest, TResponse> : BaseCommand<TResponse>, IApiCommand<TRequest, TResponse> where TRequest : class, new() where TResponse : class
    {
        public BaseResponse<TResponse> Execute(TRequest request)
        {
            if (!AllowAnonymous && CurrentApplicationUser is null)
            {
                return GetResponseFailed(Resources.Errors_AuthenticationFailed);
            }

            try
            {
                return ExecuteCore(request);
            }
            catch (Exception ex)
            {
                if (ShouldThrowExceptions)
                    throw ex;

                ExceptionLogger.LogException(ex);
                
                return GetGenericResponseFailed();
            }
        }

        protected virtual BaseResponse<TResponse> ExecuteCore(TRequest request) => GetGenericResponseFailed();
    }
}
