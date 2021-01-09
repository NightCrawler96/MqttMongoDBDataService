using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using Serilog;

namespace MqttMongoDBClient.Services
{
    public class MqttClientService : IMqttClientConnectedHandler, IMqttClientDisconnectedHandler, IMqttApplicationMessageReceivedHandler
    {
        private readonly IMqttClient client;
        private readonly IMqttClientOptions options;

        public MqttClientService(
            IMqttClient client,
            IMqttClientOptions options)
        {
            this.client = client;
            this.client.ConnectedHandler = this;
            this.client.DisconnectedHandler = this;
            this.client.ApplicationMessageReceivedHandler = this;

            this.options = options;

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
        }

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            Log.Information("Disconnected from broker service");
        }
    }
}
