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
            return _serviceHandler._authService.LoginUser<Cliente>(parameters);
        }

        [HttpPost("Register")]
        public Task<RegisterResponse> Register(RegisterParameters parameters)
        {
            return _serviceHandler._authService.RegisterUser<Trabajador>(parameters);
        }
    }
}
