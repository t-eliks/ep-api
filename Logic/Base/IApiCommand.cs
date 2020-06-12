using DataAccess.Entities.User;
using Services.Interfaces.Common;

namespace Logic.Base
{
    internal interface IApiCommand
    {
        IExceptionLogger ExceptionLogger { get; set; }
        
        ApplicationUser CurrentApplicationUser { get; set; }

        bool ShouldThrowExceptions { get; set; }

        bool AllowAnonymous { get; }
    }

    internal interface IApiCommand<TRequest, TResponse> : IApiCommand where TRequest : class, new() where TResponse : class
    {
        BaseResponse<TResponse> Execute(TRequest request);
    }

    internal interface IApiCommand<TResponse> : IApiCommand where TResponse : class
    {
        BaseResponse<TResponse> Execute();
    }
}
