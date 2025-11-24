namespace AuthService_WebAPI.Services.UserService
{
    public interface IUserService
    {
        void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
        bool VerifyPassword(string password, byte[] hash, byte[] salt);
    }
}
