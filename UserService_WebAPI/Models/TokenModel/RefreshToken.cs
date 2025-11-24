using AuthService_WebAPI.Models.UserModel;

namespace AuthService_WebAPI.Models.TokenModel
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;

        public int UserId { get; set; }
        public User User { get; set; }
    }
}



