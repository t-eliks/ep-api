using DataAccess.Models;
using Logic.Base;
using Services.Interfaces.User;

namespace Logic.Commands.User
{
    public class GetUserInfoCommand : BaseCommand<UserInfoViewModel>
    {
        private readonly IUserService usersService;

        public GetUserInfoCommand(IUserService usersService)
        {
            this.usersService = usersService;
        }

        protected override BaseResponse<UserInfoViewModel> ExecuteCore()
        {
            return GetResponseSuccess(usersService.GetUserInfoDto(CurrentApplicationUser));
        }
    }
}
