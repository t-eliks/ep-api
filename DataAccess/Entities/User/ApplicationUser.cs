using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities.User
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string AccessKey { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public override bool Equals(object obj)
        {
            var user = obj as ApplicationUser;

            if (user is null)
                return false;

            if (this.Id == default && user.Id == default)
                return base.Equals(obj);

            return this.Id.Equals(user.Id);
        }

        public static bool operator ==(ApplicationUser user1, ApplicationUser user2)
        {
            return (user1 is null && user2 is null) || !(user1 is null) && user1.Equals(user2);
        }

        public static bool operator !=(ApplicationUser user1, ApplicationUser user2)
        {
            return !(user1 == user2);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
