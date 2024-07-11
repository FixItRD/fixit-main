using fixit_main.Repositories.Templates;
using fixit_main.Repositories;
using fixit_main.Services.Templates;
using fixit_main.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace fixit_main
{
    public static class ServiceConfig
    {
        public static void ConfigureCORS(this IServiceCollection services, string MyAllowSpecifiOrigins)
        {
            string[] domains = { "", "https://localhost:7242", "http://localhost:32777", "https://localhost:32778", "http://localhost:32779" };


            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecifiOrigins,
                    policy =>
                    {
                        policy.WithOrigins(domains)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = config["JwtSettings:Issuer"],
                        ValidAudience = config["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });
        }

        public static void ConfigureAutorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("--IsAdmin", policy =>
                {
                    policy.RequireRole(new string[] { "admin" });
                });

                options.AddPolicy("--IsClient", policy =>
                {
                    policy.RequireRole(new string[] { "client" });
                });

                options.AddPolicy("--IsWorker", policy =>
                {
                    policy.RequireRole(new string[] { "worker" });
                });

                options.AddPolicy("--IsBoth", policy =>
                {
                    policy.RequireRole(new string[] { "worker", "client" });
                });
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "FixIt REST", Version = "v1.0" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContextPool<FixItDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("MainConnection")));
        }

        public static void ConfigureGraphQl(this IServiceCollection services)
        {
            services
                .AddGraphQLServer()
                .AddTypes()
                .AddFiltering()
                .AddSorting()
                .AddAuthorization()
                .RegisterDbContext<FixItDBContext>();
        }

        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<HttpClient>();
            services.AddScoped<IRepositoryHandler, RepositoryHandler>();
            services.AddScoped<IServiceHandler, ServiceHandler>();
        }

        public static void ConfigureApp(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.MapGraphQL("/api/graph");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
