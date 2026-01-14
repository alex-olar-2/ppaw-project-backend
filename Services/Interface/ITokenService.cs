using ExtractInfoIdentityDocument.Models;

namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
