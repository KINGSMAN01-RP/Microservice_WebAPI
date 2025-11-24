using AuthService_WebAPI.Data;
using AuthService_WebAPI.Models.DTOs;
using AuthService_WebAPI.Models.UserModel;
using AuthService_WebAPI.Services.TokenService;
using AuthService_WebAPI.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService_WebAPI.Controllers.Authentication
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _db;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(AuthDbContext db, IUserService userService, ITokenService tokenService)
        {
            _db = db;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(x => x.Username == dto.Username))
                return BadRequest("User already exists");

            _userService.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = dto.Role
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var user = await _db.Users.Include(r => r.RefreshTokens)
                                      .FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (user == null) return Unauthorized("Invalid user");

            if (!_userService.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid password");

            var jwt = _tokenService.CreateJwtToken(user);
            var refresh = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refresh);
            _db.SaveChanges();

            return new AuthResponseDto
            {
                Access_Token = jwt,
                Refresh_Token = refresh.Token,
                ExpiresAt = refresh.Expires
            };
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh(string refreshToken)
        {
            var user = await _db.Users.Include(r => r.RefreshTokens)
                        .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null) return Unauthorized("Invalid refresh token");

            var token = user.RefreshTokens.First(x => x.Token == refreshToken);

            if (token.IsExpired || token.IsRevoked)
                return Unauthorized("Refresh token expired");

            token.IsRevoked = true;

            var newJwt = _tokenService.CreateJwtToken(user);
            var newRefresh = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(newRefresh);
            await _db.SaveChangesAsync();

            return new AuthResponseDto
            {
                Access_Token = newJwt,
                Refresh_Token = newRefresh.Token,
                ExpiresAt = newRefresh.Expires
            };
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            var token = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
            if (token == null) return NotFound();

            token.IsRevoked = true;
            await _db.SaveChangesAsync();

            return Ok("Logged out successfully.");
        }
    }
}
