namespace fixit_main.Services.Templates
{
    public interface IHashingService
    {
        public string HashPassword(string password, out string salt);
        public string HashPasswordWithSalt(string password, string salt);
    }
}
