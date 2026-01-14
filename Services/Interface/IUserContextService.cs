namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface IUserContextService
    {
        Guid GetCurrentUserId();
        string GetCurrentUserEmail();
    }
}
