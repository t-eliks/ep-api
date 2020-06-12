using DataAccess.Entities.User;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces.User;

namespace Services.User
{
    public class RegistrationService : PasswordHasher<ApplicationUser>, IRegistrationService, IPasswordHasher<ApplicationUser>
    {
        public ApplicationUser CreateUser(RegistrationViewModel registrationViewModel)
        {
            return new ApplicationUser
            {
                Email = registrationViewModel.Email,
                FirstName = registrationViewModel.FirstName,
                LastName = registrationViewModel.LastName
            };
        }
    }
}
