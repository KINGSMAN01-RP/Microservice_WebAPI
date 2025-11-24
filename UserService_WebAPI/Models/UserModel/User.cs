
using AuthService_WebAPI.Models.TokenModel;
using System.ComponentModel.DataAnnotations;

namespace AuthService_WebAPI.Models.UserModel
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string Role { get; set; } = "Admin";
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
