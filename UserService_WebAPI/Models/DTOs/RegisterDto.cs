using System.ComponentModel.DataAnnotations;

namespace AuthService_WebAPI.Models.DTOs
{
    public class RegisterDto
    {
       [Required][MaxLength(100)] public string Username { get; set; } = string.Empty;
       [Required][MaxLength(100)] public string Password { get; set; } = string.Empty;
       [Required][MaxLength(100)] public string Role { get; set; } = "User";
    }
}
