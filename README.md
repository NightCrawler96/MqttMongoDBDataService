Project based on https://github.com/rafiulgits/mqtt-client-dotnet-core/

# MqttMongoDBDataService

MqttMongoDBDataService is an example data gathering service based on MongoDB for systems based on MQTT brokers.

Docker image: https://hub.docker.com/r/nightcrawler96/mqtt-mongodb-service

## Configurarion

Edit inside appsettings.json:

      "MqttClientConfig": {
        "MqttClientCredentials": {
          "BrokerHostUrl": "{your broker address}",
          "BrokerHostPort": 1883,
          "ClientLogin": "",
          "ClientPassword": ""
        },
        "MqttSubscribedTopic": "#"
      },
      "MongoDBClientConfig": {
        "ConnectionString": "mongodb://{your db address}",
        "Database": "{your db}",
        "Collection": "{your collection}"
      }
      
