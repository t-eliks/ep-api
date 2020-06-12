using System;
using System.Linq.Expressions;
using DataAccess.Entities.User;

namespace Services.Interfaces.Invitation
{
    public interface IInvitationService
    {
        DataAccess.Entities.Invitation GetInvitation(Expression<Func<DataAccess.Entities.Invitation, bool>> expr);

        void AcceptInvitation(ApplicationUser user, DataAccess.Entities.Invitation invitation);
    }
}
