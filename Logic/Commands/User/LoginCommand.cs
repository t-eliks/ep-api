using DataAccess.Enums;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.User;

namespace Logic.Commands.User
{
    public class LoginCommand : BaseCommand<LoginViewModel, UserInfoViewModel>
    {
        public override bool AllowAnonymous => true;

        private readonly IAuthenticationService authenticationService;

        public LoginCommand(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        protected override BaseResponse<UserInfoViewModel> ExecuteCore(LoginViewModel request)
        {
            var userDto = authenticationService.AuthenticateUser(request.Email, request.Password).Result;

            if (userDto is null)
                return GetResponseFailed(Resources.Errors_AuthenticationFailed).WithStatusCode(ResponseStatusCodes.Unauthorized);

            return GetResponseSuccess(userDto, Resources.AuthenticationSuccess);
        }
    }
}