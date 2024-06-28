using fixit_main.Services.Templates;

namespace fixit_main.Services
{
    public class ServiceHandler : IServiceHandler
    {

        public IHashingService _hashingService { get; }

        public ServiceHandler()
        {
            _hashingService = new HashingService();
        }
    }
}
