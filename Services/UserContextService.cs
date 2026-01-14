using ExtractInfoIdentityDocument.Services.Interface;

using System.Security.Claims;

namespace ExtractInfoIdentityDocument.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) throw new UnauthorizedAccessException("No user context found.");

            // Caută claim-ul NameIdentifier pus de TokenService
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim != null && Guid.TryParse(idClaim.Value, out Guid userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("User is not authenticated or ID is invalid.");
        }

        public string GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
