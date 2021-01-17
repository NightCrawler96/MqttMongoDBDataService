namespace MqttMongoDBDataService.Services.Providers
{
    public class ClientServiceProvider
    {
        public MqttClientService MqttClientService { get; }

        public ClientServiceProvider(MqttClientService mqttClientService)
        {
            MqttClientService = mqttClientService;
        }
    }
}
