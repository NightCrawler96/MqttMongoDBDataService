using MQTTnet.Client.Options;
using System;

namespace MqttMongoDBClient.Options
{
    public class MqttMongoDBClientOptionsBuilder : MqttClientOptionsBuilder
    {
        public IServiceProvider Provider { get; }

        public MqttMongoDBClientOptionsBuilder(IServiceProvider provider)
        {
            Provider = provider;
        }
    }
}
