using System;
using System.Threading.Tasks;
using DataAccess.Entities.User;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Security;
using Services.Common;
using Services.Interfaces.User;

namespace Services.User
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService usersService;
        private readonly IConfiguration configuration;

        public AuthenticationService(SignInManager<ApplicationUser> signInManager, IUserService usersService, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.usersService = usersService;
            this.configuration = configuration;
        }

        private async Task<ApplicationUser> AuthenticateCredentials(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = usersService.GetByEmail(email);

            if (user == null)
                return null;

            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
                return user;
            else
                return null;
        }

        public async Task<UserInfoViewModel> AuthenticateUser(string email, string password)
        {
            var user = await AuthenticateCredentials(email, password);

            if (user is null)
                return null;

            var secret = configuration.GetEnvironmentVariable("Secret");

            var token = JWTokenGenerator.GenerateToken(user, secret, TimeSpan.FromDays(7));

            var dto = usersService.GetUserInfoDto(user);

            dto.Token = token;

            return dto;
        }
    }
}
