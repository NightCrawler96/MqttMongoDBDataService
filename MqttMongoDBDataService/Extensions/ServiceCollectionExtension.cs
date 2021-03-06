﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttMongoDBDataService.Options;
using MqttMongoDBDataService.Services;
using MqttMongoDBDataService.Services.Providers;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;

namespace MqttMongoDBDataService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBrokerClientCredentials(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IMqttClientOptions>(serviceProvider =>
            {
                var options = new MqttClientOptionsBuilder()
                .WithClientId($"Data-Service-{Guid.NewGuid()}")
                .WithCredentials(
                    config.GetSection("MqttClientConfig:MqttClientCredentials:ClientLogin").Value,
                    config.GetSection("MqttClientConfig:MqttClientCredentials:ClientPassword").Value
                )
                .WithTcpServer(
                    config.GetSection("MqttClientConfig:MqttClientCredentials:BrokerHostUrl").Value,
                    config.GetSection("MqttClientConfig:MqttClientCredentials:BrokerHostPort").Get<int>())
                .WithCleanSession()
                .Build();

                return options;
            });
            services.AddSingleton<ClientServiceProvider>();
            services.AddSingleton<IHostedService, MqttClientService>();
            services.AddSingleton<IMqttClient>(_ => new MqttFactory().CreateMqttClient());
            services.AddSingleton<MqttClientService>();
            return services;
        }
    }
}
