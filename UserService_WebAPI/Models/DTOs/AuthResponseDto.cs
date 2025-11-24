namespace AuthService_WebAPI.Models.DTOs
{
    public class AuthResponseDto
    {
        public string? Access_Token { get; set; }
        public string? Refresh_Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
