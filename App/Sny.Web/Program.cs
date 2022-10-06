using Microsoft.AspNetCore.Components;
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

            var backendUrl = new Uri("https://localhost:7026");
            if (!builder.HostEnvironment.IsDevelopment())
            {
                backendUrl = new Uri("https://snyapi.azurewebsites.net");
            }

            builder.Services.AddScoped<IBackendProvider>(
                 (sp) => new BackendProvider(backendUrl, GetService<HttpClient>(sp), GetService<ILocalStorageService>(sp), GetService<NavigationManager>(sp)));

            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

            var host = builder.Build();

            var uc = host.Services.GetRequiredService<IUserContext>();
            await uc.Initialize();

            await host.RunAsync();
        }

        private static T GetService<T>(IServiceProvider sp)
        {
            return sp.GetService<T>() ?? throw new ApplicationException("HttpClient service not found.");
        }
    }
}