using Domain.Contracts;
using E_commerce.Apis.MiddleWares;
using Microsoft.AspNetCore.StaticFiles;

namespace E_commerce.Apis.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> UseAppMiddleware(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            await dbInitializer.IdentityInitializeAsync();

            // 2. Setup the Request Pipeline
            app.UseMiddleware<GlobalErrorHandlingMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Static files only serves KNOWN extensions - modern image formats like
            // .avif (and .webp on older runtimes) are not mapped by default and would
            // 404, so register them explicitly for the product photos.
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings[".avif"] = "image/avif";
            contentTypeProvider.Mappings[".webp"] = "image/webp";
            app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = contentTypeProvider });
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
