using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WebApi.Controllers;

[assembly: WebJobsStartup(typeof(WebApi.Startup))]
namespace WebApi
{
    public class Startup : IWebJobsStartup
    {
        public bool IsLocal => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));   
            //services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Database API", Version = "1.0" }));
            //services.AddLogging(x => x.AddConfiguration())

            Secrets secrets = new Secrets();
            if (IsLocal)
            {
                var builder = new ConfigurationBuilder();
                builder.AddUserSecrets(this.GetType().Assembly);
                IConfigurationRoot config = builder.Build();
                string key = config.GetValue<string>("DatabaseKey");
                secrets.Key = key;
            }
            else
            {
                //https://medium.com/statuscode/getting-key-vault-secrets-in-azure-functions-37620fd20a0b
                secrets.Key = Environment.GetEnvironmentVariable("DatabaseKey", EnvironmentVariableTarget.Process) ?? "";             
            }
               
            services.AddSingleton(secrets);
            services.AddSingleton<IDocumentDBRepository<EVSpecs>>(new DocumentDBRepository<EVSpecs>("bev",secrets));
            services.AddSingleton<IDocumentDBRepository<Contact>>(new DocumentDBRepository<Contact>("contact",secrets));
        }

        public void Configure(IWebJobsBuilder builder)
        {           
            ConfigureServices(builder.Services);
        }
    }
}
