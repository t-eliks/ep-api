using System;
using System.Linq;
using System.Linq.Expressions;
using DataAccess;
using DataAccess.Entities.Junctions;
using DataAccess.Entities.User;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Invitation;

namespace Services.Invitation
{
    public class InvitationService : IInvitationService
    {
        private readonly Repository repository;

        public InvitationService(Repository repository)
        {
            this.repository = repository;
        }

        public DataAccess.Entities.Invitation GetInvitation(Expression<Func<DataAccess.Entities.Invitation, bool>> expr)
        {
            return repository.Invitations
                .Read(expr)
                .Include(x => x.CreatedBy)
                .Include(x => x.Project)
                .SingleOrDefault();
        }

        public void AcceptInvitation(ApplicationUser user, DataAccess.Entities.Invitation invitation)
        {
            var junction = new ProjectToUserJunction
            {
                Collaborator = user,
                Project = invitation.Project
            };

            repository.Create(junction, user);

            repository.Delete(invitation, user);
        }
    }
}
