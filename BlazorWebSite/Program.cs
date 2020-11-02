using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CCWebSite.Controllers;
using Microsoft.Extensions.Http;
using AzureBlazorCosmosWasm.Data;

namespace BlazorWebSite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            static string functionEndpoint(WebAssemblyHostBuilder builder) =>
                builder.Configuration
                    .GetSection(nameof(TokenClient))
                    .GetValue<string>(nameof(CosmosAuthorizationMessageHandler.Endpoint));

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            /*
                        // sets up Azure Active Directory authentication and adds the 
                        // user_impersonation scope to access functions.
                        builder.Services.AddMsalAuthentication(options =>
                        {
                            options.ProviderOptions
                            .DefaultAccessTokenScopes.Add($"{functionEndpoint(builder)}user_impersonation");
                            builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                        });

                        // set up the authorization handler to inject tokens
                        builder.Services.AddTransient<CosmosAuthorizationMessageHandler>();

                        // configure the client to talk to the Azure Functions endpoint.
                        builder.Services.AddHttpClient(nameof(TokenClient),
                            client =>
                            {
                                client.BaseAddress = new Uri(functionEndpoint(builder));
                            }).AddHttpMessageHandler<CosmosAuthorizationMessageHandler>();

                        // register the client to retrieve Cosmos DB tokens.
                        builder.Services.AddTransient<TokenClient>();

            */
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            builder.Services.AddSingleton<IDocumentDBRepository<EVSpecs>>(new DocumentDBRepository<EVSpecs>("bev"));
            builder.Services.AddSingleton<BEVController>();
            builder.Services.AddSingleton<IDocumentDBRepository<Contact>>(new DocumentDBRepository<Contact>("contact"));
            builder.Services.AddSingleton<ContactController>();

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }
}
