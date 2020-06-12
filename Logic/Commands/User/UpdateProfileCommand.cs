using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.User;

namespace Logic.Commands.User
{
    public class UpdateProfileCommand : BaseCommand<UpdateProfileRequestViewModel, EmptyViewModel>
    {
        private readonly Repository repository;
        private readonly IRegistrationService registrationService;

        public UpdateProfileCommand(Repository repository, IRegistrationService registrationService)
        {
            this.repository = repository;
            this.registrationService = registrationService;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(UpdateProfileRequestViewModel request)
        {
            var user = CurrentApplicationUser;

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.PasswordHash = registrationService.HashPassword(user, request.Password);
            }

            repository.SaveChanges();

            return GetResponseSuccess(message: Resources.ProfileUpdatedSuccessfully);
        }
    }
}