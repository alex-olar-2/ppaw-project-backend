namespace ExtractInfoIdentityDocument.Models
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Cui { get; set; }
        public string SubscriptionId { get; set; }
        public string RoleId { get; set; }
        public Subscription Subscription { get; set; }
        public Role Role { get; set; }

        public ICollection<Use> Uses { get; set; } = new List<Use>();
    }
}
