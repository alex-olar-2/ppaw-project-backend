namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface IFileLoggingService
    {
        Task LogActionAsync(string action, string entityName, string details);
    }
}
