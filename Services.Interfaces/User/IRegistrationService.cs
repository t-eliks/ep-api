using DataAccess.Entities.User;
using DataAccess.Models;

namespace Services.Interfaces.User
{
    public interface IRegistrationService
    {
        ApplicationUser CreateUser(RegistrationViewModel userDto);
        string HashPassword(ApplicationUser user, string password);
    }
}
