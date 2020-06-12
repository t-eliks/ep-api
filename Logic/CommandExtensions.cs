using System.Linq;
using System.Reflection;
using Logic.Base;
using Logic.CommandFactory;
using Microsoft.Extensions.DependencyInjection;

namespace Logic
{
    public static class ApiCommandsExtension
    {
        public static void RegisterCommands(this IServiceCollection services)
        {
            services.AddScoped<ICommandFactory, CommandFactory.CommandFactory>();

            var assembly = Assembly.GetExecutingAssembly();

            foreach (var commandType in assembly.GetTypes().Where(x =>
                x.BaseType != null &&
                x.BaseType.IsGenericType &&
                (x.BaseType.GetGenericTypeDefinition() == typeof(BaseCommand<,>) ||
                x.BaseType.GetGenericTypeDefinition() == typeof(BaseCommand<>)) &&
                !x.IsAbstract))
            {
                services.AddScoped(commandType);
            }
        }
    }
}
