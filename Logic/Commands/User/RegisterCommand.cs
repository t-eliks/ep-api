using System;
using System.Linq;
using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.Invitation;
using Services.Interfaces.User;

namespace Logic.Commands.User
{
    public class RegisterCommand : BaseCommand<RegistrationViewModel, EmptyViewModel>
    {
        public override bool AllowAnonymous => true;

        private readonly Repository repository;
        private readonly IRegistrationService registrationService;
        private readonly IInvitationService invitationService;

        public RegisterCommand(Repository repository, IRegistrationService registrationService, IInvitationService invitationService)
        {
            this.repository = repository;
            this.registrationService = registrationService;
            this.invitationService = invitationService;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(RegistrationViewModel request)
        {
            if (repository.Users.Any(x => x.Email.ToLower() == request.Email.ToLower()))
                return GetResponseFailed(Resources.Errors_EmailAlreadyExists).WithStatusCode(ResponseStatusCodes.Ok);

            var user = registrationService.CreateUser(request);

            user.PasswordHash = registrationService.HashPassword(user, request.Password);

            user.UserName = request.Email;
            user.SecurityStamp = Guid.NewGuid().ToString();

            repository.Users.Add(user);

            repository.SaveChanges();

            if (request.IsInvitation)
            {
                var pendingInvitation = invitationService.GetInvitation(x => x.Email == request.Email);

                invitationService.AcceptInvitation(user, pendingInvitation);

                repository.SaveChanges();
            }

            return GetResponseSuccess(message: Resources.RegistrationSuccessful);
        }
    }
}
