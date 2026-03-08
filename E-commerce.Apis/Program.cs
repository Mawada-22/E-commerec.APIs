
using Domain.Contarcts;
using Microsoft.EntityFrameworkCore;
using Presistance.Data;
using Presistance.Data.DataSeeding;
using Presistance.Repostories;
using Services;
using ServicesAbstractions;
using Microsoft.AspNetCore.Mvc;
using E_commerce.Apis.Factories;
using E_commerce.Apis.Extensions;
using E_commerce.Apis.Extension;

namespace E_commerce.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region ServicesContainer
            // Add services to the container.
            builder.Services.AddServices(builder.Configuration);
            #endregion
            var app = builder.Build();
            // Configure the HTTP request pipeline.

            #region KestrelMiddlewareCongif
            await app.UseAppMiddleware();
            #endregion

            app.Run();

         
        }
    }
}
