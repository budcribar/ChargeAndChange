using MatBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Client.ViewModels;
using Radzen;
using BlazorPro.BlazorSize;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var baseAddress = builder.Configuration["BaseAddress"] ?? builder.HostEnvironment.BaseAddress;
            var url = new Uri(builder.HostEnvironment.BaseAddress);
            builder.Services.AddSingleton(new SwaggerClient(baseAddress, new HttpClient { BaseAddress=url }));
            builder.Services.AddSingleton<AppVersionInfo>();
            builder.Services.AddSingleton<IBEVViewModel,BEVViewModel>();
            builder.Services.AddSingleton<IContactViewModel, ContactViewModel>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
                options.ProviderOptions.LoginMode = "redirect";
            });
            builder.Services.AddMatBlazor();
            builder.Services.AddMediaQueryService();
            await builder.Build().RunAsync();
        }
    }
}
