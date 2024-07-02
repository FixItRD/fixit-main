using fixit_main.Repositories.Templates;
using fixit_main.Services.Templates;

namespace fixit_main.Services
{
    public class ServiceHandler : IServiceHandler
    {

        public IHashingService _hashingService { get; }

        public IAuthService _authService { get; }

        public ServiceHandler(IRepositoryHandler repositoryHandler, HttpClient httpClient)
        {
            _hashingService = new HashingService();
            _authService = new AuthService(repositoryHandler, _hashingService, httpClient);
        }
    }
}
