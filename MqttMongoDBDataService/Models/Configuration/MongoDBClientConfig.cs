namespace MqttMongoDBClient.Models.Configuration
{
    public class MongoDBClientConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}
