using Domain.Contarcts;
using Domain.Entities.Idenetity;
using E_commerce.Apis.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistance.Data;
using Presistance.Data.DataSeeding;
using Presistance.Idenetity;
using Presistance.Repostories;
using Services;
using ServicesAbstractions;
using StackExchange.Redis;

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
            services.AddSwaggerGen();
            services.AddSingleton<IConnectionMultiplexer>(_=> ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));
            services.AddScoped<IBasketRepo, BasketRepo>();
            return services;
        }
        public static IServiceCollection AddIdenetityServices (this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(opt =>{
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<IdenetityAppDBcontext>();
            return services;
        }
    }
}
