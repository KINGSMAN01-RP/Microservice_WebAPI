using AuthService_WebAPI.Models.TokenModel;
using AuthService_WebAPI.Models.UserModel;

namespace AuthService_WebAPI.Services.TokenService
{
    public interface ITokenService
    {
        string CreateJwtToken(User user);
        RefreshToken GenerateRefreshToken();
    }
}
