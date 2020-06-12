using Microsoft.Extensions.DependencyInjection;
using Services.Assignment;
using Services.Common;
using Services.Interfaces.Assignment;
using Services.Interfaces.Common;
using Services.Interfaces.Invitation;
using Services.Interfaces.Notification;
using Services.Interfaces.Project;
using Services.Interfaces.User;
using Services.Invitation;
using Services.Notification;
using Services.Project;
using Services.User;

namespace Services
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IInvitationService, InvitationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IExceptionLogger, ExceptionLogger>();
        }
    }
}
