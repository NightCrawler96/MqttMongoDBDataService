using MQTTnet.Client.Options;
using System;

namespace MqttMongoDBDataService.Options
{
    public class MqttMongoDBDataServiceOptionsBuilder : MqttClientOptionsBuilder
    {
        public IServiceProvider Provider { get; }

        public MqttMongoDBDataServiceOptionsBuilder(IServiceProvider provider)
        {
            Provider = provider;
        }
    }
}
