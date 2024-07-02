using fixit_main.Models;
using fixit_main.Models.Templates;

namespace fixit_main.Services.Templates
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginUser<T>(LogInParameters LogInParameters) where T : class, IUser;
        Task<RegisterResponse> RegisterUser<T>(RegisterParameters registerParameters) where T : class, IUser;
    }
}
