using System;

namespace DataAccess.Dtos
{
    public class PendingInvitationDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public string InvitedByFirstName { get; set; }

        public string InvitedByLastName { get; set; }

        public string ProjectName { get; set; }

        public bool AccountExists { get; set; }
    }
}
