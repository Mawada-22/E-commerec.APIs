using Domain.Contracts;
using Domain.Entities.Identity;
using E_commerce.Apis.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Data.DataSeeding;
using Persistence.Identity;
using Persistence.Repositories;
using Services;
using ServicesAbstractions;
using Shared;
using StackExchange.Redis;
using System.Text;

namespace E_commerce.Apis.Extensions
{
    public static class AddServicesToContainerExtensions
    {
        // Add IConfiguration configuration as a parameter
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

            // The Angular storefront runs on localhost:4200 and calls this API
            // cross-origin; without CORS every browser call is blocked.
            services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .WithOrigins("http://localhost:4200", "https://localhost:4200")));

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AssemblyReference).Assembly);
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IServiceManager, ServiceManager>();

            

            // Use the 'configuration' parameter here
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<IdentityAppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
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
            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<IBasketRepo, BasketRepo>();
            return services;
        }
        public static IServiceCollection AddIdentityServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>(opt =>{
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<IdentityAppDbContext>();
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
