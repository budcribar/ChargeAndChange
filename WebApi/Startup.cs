using CCWebSite.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
//using System.Text.Json.Serialization;

[assembly: WebJobsStartup(typeof(WebApi.Startup))]
namespace WebApi 
{
    public class Startup : IWebJobsStartup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));   
            //services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Database API", Version = "1.0" }));
            services.AddSingleton<IDocumentDBRepository<CCWebSite.Controllers.EVSpecs>>(new DocumentDBRepository<CCWebSite.Controllers.EVSpecs>("bev"));
            services.AddSingleton<IDocumentDBRepository<CCWebSite.Controllers.Contact>>(new DocumentDBRepository<CCWebSite.Controllers.Contact>("contact"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
       

        public void Configure(IWebJobsBuilder builder)
        {
            ConfigureServices(builder.Services);
        }
    }
}
