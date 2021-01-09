using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
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
        private readonly IConfiguration config;

        public MqttClientService(
            IMqttClient client,
            IMqttClientOptions options,
            IConfiguration config)
        {
            this.client = client;
            this.client.ConnectedHandler = this;
            this.client.DisconnectedHandler = this;
            this.client.ApplicationMessageReceivedHandler = this;

            this.options = options;
            this.config = config;
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            try
            {
                var message = Encoding.Unicode.GetString(eventArgs.ApplicationMessage.Payload);
                Log.Information($"Received message: {message}");
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Couldn't read received message");
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
