namespace ExtractInfoIdentityDocument.Models
{
    public class IdentityCard : BaseEntity
    {
        public string Cnp { get; set; }
        public string Series { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }

    }
}
