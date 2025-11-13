namespace ExtractInfoIdentityDocument.Models
{
    public class Subscription : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsDefault { get; set; }

    }
}
