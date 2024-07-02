namespace fixit_main.Services.Templates
{
    public interface IServiceHandler
    {
        IHashingService _hashingService { get; }
        IAuthService _authService { get; }
    }
}
