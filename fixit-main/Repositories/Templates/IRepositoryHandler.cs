namespace fixit_main.Repositories.Templates
{
    public interface IRepositoryHandler
    {
        IAuthRepository _authRepository { get; }
        IUserRepository _userRepository { get; }
    }
}
