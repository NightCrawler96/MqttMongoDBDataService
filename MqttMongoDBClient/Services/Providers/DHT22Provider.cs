using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MqttMongoDBDataService.Models;
using MqttMongoDBDataService.Models.Configuration;
using System;

namespace MqttMongoDBDataService.Services.Providers
{
    public class DHT22Provider
    {
        private readonly IMongoCollection<DHT22> models;

        public DHT22Provider(IMongoClient client, MongoDBClientConfig clientConfig)
        {
            this.models = client.GetDatabase(clientConfig.Database)
                                .GetCollection<DHT22>(clientConfig.Collection);
        }

        public void Save(DHT22 model)
        {
            this.models.InsertOne(model);
        }
    }
}
