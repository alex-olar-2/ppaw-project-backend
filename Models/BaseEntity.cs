namespace ExtractInfoIdentityDocument.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsVisible { get; set; }
    }
}
