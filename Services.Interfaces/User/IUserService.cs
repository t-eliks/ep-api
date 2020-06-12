using DataAccess.Entities.User;
using DataAccess.Models;

namespace Services.Interfaces.User
{
    public interface IUserService
    {
        ApplicationUser GetByEmail(string email);
        ApplicationUser GetById(int id);
        UserInfoViewModel GetUserInfoDto(ApplicationUser user);
    }
}
