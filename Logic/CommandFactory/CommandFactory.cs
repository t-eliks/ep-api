using System;
using DataAccess.Entities.User;
using Logic.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces.Common;

namespace Logic.CommandFactory
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IServiceProvider provider;
        private readonly IHostingEnvironment env;
        private readonly IExceptionLogger exceptionLogger;

        public CommandFactory(IServiceProvider provider, IHostingEnvironment env, IExceptionLogger exceptionLogger)
        {
            this.provider = provider;
            this.env = env;
            this.exceptionLogger = exceptionLogger;
        }

        public TCommand ResolveCommand<TCommand>(ApplicationUser user) where TCommand : class
        {
            var service = provider.GetRequiredService<TCommand>();

            if (!(service is IApiCommand command))
                throw new ArgumentException("Type argument given is not a valid command.");

            // command.ShouldThrowExceptions = env.IsDevelopment();
            command.ShouldThrowExceptions = false;
            command.CurrentApplicationUser = user;
            command.ExceptionLogger = exceptionLogger;

            return service;
        }
    }
}
