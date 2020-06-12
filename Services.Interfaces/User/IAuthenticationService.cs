using System.Threading.Tasks;
using DataAccess.Models;

namespace Services.Interfaces.User
{
    public interface IAuthenticationService
    {
        Task<UserInfoViewModel> AuthenticateUser(string email, string password);
    }
}
