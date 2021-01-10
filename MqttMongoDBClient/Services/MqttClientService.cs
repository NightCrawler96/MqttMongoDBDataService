using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MqttMongoDBClient.Models;
using MqttMongoDBClient.Services.Providers;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using Newtonsoft.Json;
using Serilog;

namespace MqttMongoDBClient.Services
{
    public class MqttClientService : IMqttClientConnectedHandler,
                                     IMqttClientDisconnectedHandler,
                                     IMqttApplicationMessageReceivedHandler,
                                     IHostedService
    {
        private readonly IMqttClient client;
        private readonly IMqttClientOptions options;
        private readonly DHT22Provider dht22Provider;
        private readonly IConfiguration config;

        public MqttClientService(
            IMqttClient client,
            IMqttClientOptions options,
            DHT22Provider dht22Provider,
            IConfiguration config)
        {
            this.client = client;
            this.client.ConnectedHandler = this;
            this.client.DisconnectedHandler = this;
            this.client.ApplicationMessageReceivedHandler = this;

            this.options = options;
            this.dht22Provider = dht22Provider;
            this.config = config;
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            try
            {
                var message = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
                Log.Information($"Received message: {message}");
                var value = JsonConvert.DeserializeObject<DHT22>(message);
                this.dht22Provider.Save(value);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Couldn't read received message or deserialize object");
            }
            
        }

        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            Log.Information("Connected with broker service");
            await this.client.SubscribeAsync(
                this.config.GetSection("MqttClientConfig:MqttSubscribedTopic").Get<string>()
            );
        }

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            Log.Information("Disconnected from broker service");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.client.ConnectAsync(this.options, cancellationToken);
            if (!this.client.IsConnected)
            {
                await this.client.ReconnectAsync();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var disconnectOptions = new MqttClientDisconnectOptions
            {
                ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                ReasonString = "Diconnection"
            };
            await this.client.DisconnectAsync(disconnectOptions, cancellationToken);
        }
    }
}
