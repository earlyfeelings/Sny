using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sny.Web.Services.BackendProvider;
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
                builder.Services.AddScoped<IBackendProvider>((sp) => new BackendProvider(new Uri("https://localhost:7026"), true));
            }
            else
            {
                builder.Services.AddScoped<IBackendProvider>((sp) => new BackendProvider(new Uri("https://snyapi.azurewebsites.net"), false));
            }
            builder.Services.AddScoped<IUserContext, UserContext>();

            await builder.Build().RunAsync();
        }
    }
}