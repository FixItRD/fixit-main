using fixit_main.Models;

namespace fixit_main.Repositories.Templates
{
    public interface IAuthRepository
    {
        Task<string> IsValidClient(LogInParameters LogInParameters);
        Task<string> IsValidWorker(LogInParameters LogInParameters);
        Task<bool> RegisterClient(RegisterParameters registerParameters);
    }
}
