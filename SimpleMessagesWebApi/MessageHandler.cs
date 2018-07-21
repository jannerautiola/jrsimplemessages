using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace SimpleMessagesWebApi
{
    public class MessageHandler
    {

        private const string EndpointUrl = "<your endpoint URL>";
        private const string PrimaryKey = "<your primary key>";
        private DocumentClient client;

        private const string DatabaseName = "Messages";
        private const string CollectionName = "MessagesCollection";


        public MessageHandler()
        {
            var config = new JsonConfigurationFileReader("ApiKeys.json");

            var endpointUrl = config.GetConfigValue("AzureCosmosDbEndpoint");
            var key = config.GetConfigValue("AzureCosmosDbKey");

            this.client = new DocumentClient(new Uri(endpointUrl), key);
        }

        public async Task SaveMessage(string key, Dictionary<string, string> values)
        {
            await this.Initialize();

            Message message = new Message()
            {
                id = key + "_" + Guid.NewGuid().ToString(),
                key = key,
                timestamp = DateTime.UtcNow,
                values = values
            };

            await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), message);
        }

        public async Task<List<Message>> GetMessages(string key)
        {
            await this.Initialize();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            var messages = this.client.CreateDocumentQuery<Message>(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions)
                .Where(m => m.key == key)
                .OrderByDescending(m => m.timestamp)
                .Take(5)
                .ToList();

            return messages;
        }

        private async Task Initialize()
        {
            //await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });
            //await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = CollectionName });
        }

        public class Message
        {
            public string id { get; set; }
            public string key { get; set; }
            public DateTime timestamp { get; set; }
            public Dictionary<string, string> values { get; set; }
        }
    }
}
