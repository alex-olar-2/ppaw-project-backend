namespace ExtractInfoIdentityDocument.Models
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Cui { get; set; }
        public Guid SubscriptionId { get; set; }
        public Guid RoleId { get; set; }
        public Subscription Subscription { get; set; }
        public Role Role { get; set; }

    }
}
