namespace DataAccess.Entities
{
    public class Invitation : BaseEntity
    {
        public virtual Project Project { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
