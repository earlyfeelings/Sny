using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sny.Web.Services.BackendProvider;
using Sny.Web.Services.LocalStorageService;
using Sny.Web.Services.UserContext;

namespace Sny.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
 
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            if (builder.HostEnvironment.IsDevelopment())
            {
                builder.Services.AddScoped<IBackendProvider>(
                    (sp) =>
                        new BackendProvider(new Uri("https://localhost:7026"),
                        sp.GetService<HttpClient>() ?? throw new ApplicationException("HttpClient service not found.")
                        )
                    );
            }
            else
            {
                builder.Services.AddScoped<IBackendProvider>(
                    (sp) =>
                        new BackendProvider(new Uri("https://snyapi.azurewebsites.net"),
                        sp.GetService<HttpClient>() ?? throw new ApplicationException("HttpClient service not found.")
                        )
                    );
            }
            
            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

            var host = builder.Build();

            var uc = host.Services.GetRequiredService<IUserContext>();
            await uc.Initialize();

            await host.RunAsync();
        }
    }
}