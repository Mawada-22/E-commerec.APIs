
using Domain.Contarcts;
using Microsoft.EntityFrameworkCore;
using Presistance.Data;
using Presistance.Data.DataSeeding;
using Presistance.Repostories;
using AutoMapper;
using System;
using Services;
using Microsoft.Extensions.DependencyInjection;
using ServicesAbstractions;

namespace E_commerce.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region ServicesContainer
            // Add services to the container.

            builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
            builder.Services.AddScoped<IDbInitializer, DBIntialaizer>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(AssembelyRefernce).Assembly);
            builder.Services.AddScoped<IServiceManger, ServiceManger>();
            builder.Services.AddDbContext<StoreDbcontext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            
            
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
          



            var app = builder.Build();
            await IntializeDbAsync(app);

            #endregion

            // Configure the HTTP request pipeline.

            #region KestrelMiddlewareCongif
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            //use images 
            app.UseStaticFiles();

            app.MapControllers(); 
            #endregion

            app.Run();

            async Task IntializeDbAsync(WebApplication app)
            {
                // create object from type imblemnts IDIntializer -- DI Explicitly 
                using var scope = app.Services.CreateScope();
                var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await dbIntializer.IntialaizerAsync();
            }
        }
    }
}
