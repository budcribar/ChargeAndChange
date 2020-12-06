using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var baseAddress = builder.Configuration["BaseAddress"] ?? builder.HostEnvironment.BaseAddress;
            var url = new Uri(builder.HostEnvironment.BaseAddress);
            builder.Services.AddSingleton(new SwaggerClient(baseAddress, new HttpClient { BaseAddress=url }));
            builder.Services.AddSingleton<AppVersionInfo>();
           await builder.Build().RunAsync();
        }
    }
}
