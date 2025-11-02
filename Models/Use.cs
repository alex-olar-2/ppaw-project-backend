namespace ExtractInfoIdentityDocument.Models
{
    public class Use : BaseEntity
    {
        public Guid UserId { get; set; }
        public bool IsSucceeded { get; set; }
        public Guid IdentityCardId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public User User { get; set; }
        public IdentityCard IdentityCard { get; set; }
    }
}
