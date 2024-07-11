using fixit_main.Models;
using fixit_main.Models.Templates;
using fixit_main.Repositories;
using fixit_main.Repositories.Templates;
using fixit_main.Services.Templates;

namespace fixit_main.Services
{
    public class AuthService : IAuthService
    {
        IRepositoryHandler _repositoryHandler;
        IHashingService _hashingService;
        HttpClient _httpClient;

        public AuthService(IRepositoryHandler repositoryHandler, IHashingService hashingService, HttpClient httpClient)
        {
            _repositoryHandler = repositoryHandler;
            _hashingService = hashingService;
            _httpClient = httpClient;
        }

        Task<LoginResponse> IAuthService.LoginUser<T>(LogInParameters LogInParameters)
        {
            return Task.Run(LoginResponse () =>
            {
                try
                {
                    CrudOperationResponse existingUser = _repositoryHandler._userRepository.ReadUserAsync<T>(LogInParameters.Email).Result;

                    if (existingUser.Items is null)
                    {
                        return new LoginResponse(1, "User not found");
                    }

                    T currentUser = (T)existingUser.Items[0];

                    string pass = _hashingService.HashPasswordWithSalt(LogInParameters.Password, currentUser.Salt);

                    if (pass == currentUser.Password)
                    {
                        string role = typeof(T) == typeof(Cliente) ? "client" : "worker";

                        string token = _httpClient.PostAsJsonAsync("https://fixit-token-handler20240711122232.azurewebsites.net/api/v1/Token", new UserClaimsModel()
                        {
                            UserId = currentUser.ID,
                            Name = currentUser.Nombre,
                            Role = role
                        }).Result.Content.ReadAsStringAsync().Result;

                        return new LoginResponse(0, message: "User found", token);
                    }
                    else
                    {
                        return new LoginResponse(2, message: "Invalid password");
                    }
                }
                catch (Exception ex)
                {
                    return new LoginResponse(3, message: ex.Message + " | " + ex.StackTrace);
                }
            });
        }

        Task<RegisterResponse> IAuthService.RegisterUser<T>(RegisterParameters registerParameters)
        {
            return Task.Run(RegisterResponse () =>
            {
                try
                {
                    CrudOperationResponse existingUser = _repositoryHandler._userRepository.ReadUserAsync<T>(registerParameters.Email).Result;

                    if (existingUser.Items is not null)
                    {
                        return new RegisterResponse(1, "User already exists");
                    }

                    T user = (T)Activator.CreateInstance(typeof(T), new object[] { });
                    user.Email = registerParameters.Email;
                    user.Password = _hashingService.HashPassword(registerParameters.Password, out string salt);
                    user.Salt = salt;
                    user.Nombre = registerParameters.Name;

                    Task<CrudOperationResponse> crudOperation = _repositoryHandler._userRepository.CreateUserAsync<T>(user);

                    if (crudOperation.Result.Success)
                    {
                        T currentUser = (T)_repositoryHandler._userRepository.ReadUserAsync<T>(registerParameters.Email).Result.Items[0];
                        string role = typeof(T) == typeof(Cliente) ? "client" : "worker";

                        string token = _httpClient.PostAsJsonAsync("https://fixit-token-handler20240711122232.azurewebsites.net/api/v1/Token", new UserClaimsModel()
                        {
                            UserId = currentUser.ID,
                            Name = currentUser.Nombre,
                            Role = role
                        }).Result.Content.ReadAsStringAsync().Result;

                        return new RegisterResponse(0, message: "User created successfully", token);
                    }
                    else
                    {
                        return new RegisterResponse(2, message: "User creation failed | " + crudOperation.Result.Message);
                    }
                }
                catch (Exception ex)
                {
                    return new RegisterResponse(3, message:ex.Message + " | " + ex.StackTrace);
                }
            });
        }
    }
}
