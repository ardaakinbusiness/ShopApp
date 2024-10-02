using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace ShopApp.WebUI.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder CustomStaticFiles(this IApplicationBuilder app)
        {

            //C:\Users\emre\source\repos\CSharpKursu\WEBYAZILIM\ShopApp\ShopApp.WebUI\node_modules
            var path = Path.Combine(Directory.GetCurrentDirectory(), "node_modules");

            var options = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/modules"
            };

            app.UseStaticFiles(options);

            return app;
        }
    }
}
