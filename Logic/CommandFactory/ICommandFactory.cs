using DataAccess.Entities.User;

namespace Logic.CommandFactory
{
    public interface ICommandFactory
    {
        TCommand ResolveCommand<TCommand>(ApplicationUser user) where TCommand : class;
    }
}
