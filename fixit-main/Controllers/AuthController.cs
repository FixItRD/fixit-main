using fixit_main.Models;
using fixit_main.Services.Templates;
using Microsoft.AspNetCore.Mvc;

namespace fixit_main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IServiceHandler _serviceHandler;

        public AuthController(ILogger<AuthController> logger, IServiceHandler serviceHandler)
        {
            _logger = logger;
            _serviceHandler = serviceHandler;
        }

        [HttpPost("Login")]
        public Task<LoginResponse> Login(LogInParameters parameters)
        {
            if (parameters.Role == "client")
                return _serviceHandler._authService.LoginUser<Cliente>(parameters);
            else if (parameters.Role == "worker")
                return _serviceHandler._authService.LoginUser<Trabajador>(parameters);
            else
                return Task.Run(() => new LoginResponse(4, "Invalid role"));
        }

        [HttpPost("Register")]
        public Task<RegisterResponse> Register(RegisterParameters parameters)
        {
            if (parameters.Role == "client")
                return _serviceHandler._authService.RegisterUser<Cliente>(parameters);
            else if (parameters.Role == "worker")
                return _serviceHandler._authService.RegisterUser<Trabajador>(parameters);
            else
                return Task.Run(() => new RegisterResponse(4, "Invalid role"));
        }
    }
}
