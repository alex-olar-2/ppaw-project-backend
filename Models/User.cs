namespace ExtractInfoIdentityDocument.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Cui { get; set; }
        public Guid SubscriptionId { get; set; }
        public Guid RoleId { get; set; }
        public Subscription Subscription { get; set; }
        public Role Role { get; set; }

    }
}
