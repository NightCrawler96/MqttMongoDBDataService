using Microsoft.Extensions.DependencyInjection;
using MqttMongoDBClient.Services;

namespace MqttMongoDBClient.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBrokerClientCredentials(this IServiceCollection services)
        {
            services.AddTransient<MqttClientService>();
            return services;
        }
    }
}
