using fixit_main.Models;
using fixit_main.Repositories;
using fixit_main.Repositories.Templates;
using fixit_main.Services;
using fixit_main.Services.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace fixit_main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            // Configurations
            services.ConfigureCORS("_MyAllowSpecifiOrigins");
            services.ConfigureJWT(builder.Configuration);
            services.ConfigureSwagger();
            services.ConfigureAutorization();
            services.ConfigureDbContext(builder.Configuration);
            services.ConfigureGraphQl();
            services.InjectServices();

            // Add services to the container.
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var app = builder.Build();

            app.ConfigureApp();
        }
    }
}
