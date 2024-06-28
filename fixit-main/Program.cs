
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

            HttpClient client = new HttpClient();
            FixItDBContext fixItDbContext = new FixItDBContext(builder.Configuration.GetConnectionString("MainConnection"));
            IRepositoryHandler repositoryHandler = new RepositoryHandler(fixItDbContext, client);

            // Configuraciones
            builder.Services.ConfigureCORS("_MyAllowSpecifiOrigins");
            builder.Services.ConfigureJWT(builder.Configuration);
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureAutorization();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configuring services
            builder.Services.AddSingleton(fixItDbContext);
            builder.Services.AddSingleton(client);
            builder.Services.AddScoped<IRepositoryHandler, RepositoryHandler>();
            builder.Services.AddScoped<IServiceHandler, ServiceHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
