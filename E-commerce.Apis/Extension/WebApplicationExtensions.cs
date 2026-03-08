using Domain.Contarcts;
using E_commerce.Apis.MiddleWares;

namespace E_commerce.Apis.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> UseAppMiddleware(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.IntialaizerAsync();

            // 2. Setup the Request Pipeline
            app.UseMiddleware<GlobalErrorHandlingMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
