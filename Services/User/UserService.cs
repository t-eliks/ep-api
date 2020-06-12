using System;
using System.Linq;
using DataAccess;
using DataAccess.Entities.User;
using DataAccess.Models;
using Services.Interfaces.User;

namespace Services.User
{
    public class UserService : IUserService
    {
        private readonly Repository repository;

        public UserService(Repository repository)
        {
            this.repository = repository;
        }

        public ApplicationUser GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Email cannot be empty.");

            return repository.Users.SingleOrDefault(x => x.Email == email);
        }

        public ApplicationUser GetById(int id)
        {
            return repository.Users.SingleOrDefault(x => x.Id == id);
        }

        public UserInfoViewModel GetUserInfoDto(ApplicationUser user)
        {
            return new UserInfoViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
