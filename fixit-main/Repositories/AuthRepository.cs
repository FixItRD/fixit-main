using fixit_main.Models;
using fixit_main.Repositories.Templates;
using fixit_main.Services;
using fixit_main.Services.Templates;
using Microsoft.EntityFrameworkCore;

namespace fixit_main.Repositories
{
    public class AuthRepository
    {
        //private readonly FixItDBContext _context;
        //private readonly IServiceHandler _serviceHandler;
        //private readonly HttpClient _httpClient;

        //public AuthRepository(FixItDBContext context, HttpClient httpClient)
        //{
        //    _context = context;
        //    _serviceHandler = new ServiceHandler();
        //    _httpClient = httpClient;
        //}

        //public async Task<string> IsValidClient(LogInParameters LogInParameters)
        //{

        //    Cliente currentClient = _context.Cliente.Where(u => u.Email == LogInParameters.Email).FirstOrDefault();

        //    if (currentClient is not null)
        //    {

        //        string pass = _serviceHandler._hashingService.HashPasswordWithSalt(LogInParameters.Password, currentClient.Salt);

        //        string dbPass = currentClient.Password;

        //        if (pass == dbPass)
        //        {
        //            return _httpClient.PostAsJsonAsync("https://localhost:32776/api/v1/Token", new UserClaimsModel()
        //            {
        //                UserId = currentClient.ID,
        //                Name = currentClient.Nombre,
        //                Role = "client"
        //            }).Result.Content.ReadAsStringAsync().Result;
        //        }
        //        else
        //        {
        //            return "not found";
        //        }
        //    }
        //    return "not found";

        //}

        //public async Task<string> IsValidWorker(LogInParameters LogInParameters)
        //{

        //    Trabajador currentWorker = _context.Trabajador.Where(u => u.Email == LogInParameters.Email).FirstOrDefault();

        //    if (currentWorker is not null)
        //    {
        //        string pass = _serviceHandler._hashingService.HashPasswordWithSalt(LogInParameters.Password, currentWorker.Salt);

        //        string dbPass = currentWorker.Password;

        //        if (pass == dbPass)
        //        {
        //            return _httpClient.PostAsJsonAsync("https://localhost:32776/api/v1/Token", new UserClaimsModel()
        //            {
        //                UserId = currentWorker.ID,
        //                Name = currentWorker.Nombre,
        //                Role = "worker"
        //            }).Result.Content.ReadAsStringAsync().Result;
        //        }
        //        else
        //        {
        //            return "not found";
        //        }
        //    }
        //    return "not found";
        //}

        //public async Task<bool> RegisterClient(RegisterParameters registerParameters)
        //{
        //    string salt;
        //    string hashedPassword = _serviceHandler._hashingService.HashPassword(registerParameters.Password, out salt);

        //    _context.Cliente.Add(new Cliente
        //    {
        //        Nombre = registerParameters.Name,
        //        Email = registerParameters.Email,
        //        Fecha_Registro = DateTime.Now,
        //        Password = hashedPassword,
        //        Salt = salt
        //    });

        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (Exception err)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
