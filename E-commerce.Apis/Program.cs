using E_commerce.Apis.Extensions;
using E_commerce.Apis.Extensions;

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
            builder.Services.AddIdentityServices(builder.Configuration);
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
