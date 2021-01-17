using System;

namespace MqttMongoDBDataService.Models
{
    public class DHT22
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime Received { get; set; }
    }
}
