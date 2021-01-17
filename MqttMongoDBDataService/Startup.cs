using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
<<<<<<< HEAD:MqttMongoDBDataService/Startup.cs
using MqttMongoDBDataService.Extensions;
using MqttMongoDBDataService.Models.Configuration;
using MqttMongoDBDataService.Services.Providers;
=======
using MqttMongoDBClient.Extensions;
using MqttMongoDBClient.Models.Configuration;
using MqttMongoDBClient.Services.Providers;
>>>>>>> origin/master:MqttMongoDBClient/Startup.cs

namespace MqttMongoDBDataService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddBrokerClientCredentials(Configuration);

            services.Configure<MongoDBClientConfig>(
                Configuration.GetSection(nameof(MongoDBClientConfig)));
            services.AddTransient<MongoDBClientConfig>(
                serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDBClientConfig>>().Value);
            services.AddTransient<IMongoClient>(
                serviceProvider => new MongoClient(serviceProvider.GetService<MongoDBClientConfig>().ConnectionString));
            services.AddTransient<DHT22Provider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
