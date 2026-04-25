using LibraryApi.Models;

namespace LibraryApi.Services
{
    public class TokenService : ITokenService
    {
        public TokenService() { }

        public (string Token, DateTime ExpiresAt) GenerateToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}
