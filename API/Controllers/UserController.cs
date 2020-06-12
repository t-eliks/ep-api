using DataAccess.Models;
using Logic.Base;
using Logic.CommandFactory;
using Logic.Commands.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ExtendedController
    {
        public UserController(ICommandFactory commandFactory) : base(commandFactory) { }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<BaseResponse<UserInfoViewModel>> Login([FromBody] LoginViewModel model)
        {
            return GetResult(GetCommand<LoginCommand>().Execute(model));
        }

        [Authorize]
        [HttpGet]
        public ActionResult<BaseResponse<UserInfoViewModel>> VerifyAuthorization()
        {
            return GetResult(GetCommand<GetUserInfoCommand>().Execute()); // If unauthorized, 401 gets returned due to [Authorize]
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public ActionResult<BaseResponse<EmptyViewModel>> Register([FromBody] RegistrationViewModel request)
        {
            return GetResult(GetCommand<RegisterCommand>().Execute(request));
        }

        [Authorize]
        [HttpPut]
        [Route("update-profile")]
        public ActionResult<BaseResponse<EmptyViewModel>> UpdateProfile([FromBody] UpdateProfileRequestViewModel viewModel)
        {
            return GetResult(GetCommand<UpdateProfileCommand>().Execute(viewModel));
        }

        [Authorize]
        [HttpDelete]
        [Route("delete-profile")]
        public ActionResult<BaseResponse<EmptyViewModel>> DeleteProfile()
        {
            return GetResult(GetCommand<DeleteProfileCommand>().Execute());
        }
        
        [Authorize]
        [HttpGet]
        [Route("dashboard")]
        public ActionResult<BaseResponse<DashboardViewModel>> GetDashboard()
        {
            return GetResult(GetCommand<GetDashboardCommand>().Execute());
        }
    }
}