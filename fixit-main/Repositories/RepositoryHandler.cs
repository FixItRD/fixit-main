using fixit_main.Repositories.Templates;

namespace fixit_main.Repositories
{
    public class RepositoryHandler : IRepositoryHandler
    {
        public IAuthRepository _authRepository { get; }
        public IUserRepository _userRepository { get; }

        public RepositoryHandler(FixItDBContext context, HttpClient httpClient)
        {
            _authRepository = new AuthRepository(context, httpClient);
            _userRepository = new UserRepository();
        }
    }
}
