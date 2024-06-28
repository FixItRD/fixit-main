using fixit_main.Models;
using fixit_main.Repositories.Templates;
using fixit_main.Services.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fixit_main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FixItDBContext _dbContext;
        private readonly ILogger<AuthController> _logger;
        private readonly IServiceHandler _serviceHandler;
        private readonly IRepositoryHandler _repositoryHandler;

        public AuthController(ILogger<AuthController> logger, FixItDBContext dbContext, IServiceHandler serviceHandler, IRepositoryHandler repositoryHandler)
        {
            _logger = logger;
            _dbContext = dbContext;
            _serviceHandler = serviceHandler;
            _repositoryHandler = repositoryHandler;
        }

        [HttpPost("Login")]
        public string Login(LogInParameters parameters)
        {
            return _repositoryHandler._authRepository.IsValidClient(parameters).Result.ToString();
        }

        [HttpPost("Register")]
        public string Register(RegisterParameters parameters)
        {
            return _repositoryHandler._authRepository.RegisterClient(parameters).Result.ToString();
        }
    }
}
