using Domain.Contarcts;
using Domain.Entities.Idenetity;
using E_commerce.Apis.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Presistance.Data;
using Presistance.Data.DataSeeding;
using Presistance.Idenetity;
using Presistance.Repostories;
using Services;
using ServicesAbstractions;
using Shared;
using StackExchange.Redis;
using System.Text;

namespace E_commerce.Apis.Extension
{
    public static class AddServicesToContainerExtintion
    {
        // Add IConfiguration configuration as a parameter
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
            services.AddScoped<IDbInitializer, DBIntialaizer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AssembelyRefernce).Assembly);
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IAthenticationService, AthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IServiceManger, ServiceManger>();

            

            // Use the 'configuration' parameter here
            services.AddDbContext<StoreDbcontext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<IdenetityAppDBcontext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdenetityConnection"));
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Enter JWT Token"
                    });

                options.AddSecurityRequirement(
                    new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
                    });
            });

            services.AddSingleton<IConnectionMultiplexer>(_=> ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));
            services.AddScoped<IBasketRepo, BasketRepo>();
            return services;
        }
        public static IServiceCollection AddIdenetityServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>(opt =>{
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<IdenetityAppDBcontext>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters =
           new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,

               ValidIssuer = jwtOptions!.Issuer,
               ValidAudience = jwtOptions.Audience,

               IssuerSigningKey =
                   new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
           };
   });

            return services;
        }
    }
}
