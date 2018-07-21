using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace SimpleMessagesSignalRFunction
{
    public static class CosmosTriggerFunctionToSignalR
    {
        [FunctionName("CosmosTriggerFunctionToSignalR")]
        public static void Run([CosmosDBTrigger(
            databaseName: "Messages",
            collectionName: "MessagesCollection",
            ConnectionStringSetting = "CosmosDBConnectionString",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, TraceWriter log)
        {
            if (input != null && input.Count > 0)
            {
                log.Verbose("Documents modified " + input.Count);
                log.Verbose("First document Id " + input[0].Id);


                Task.Factory.StartNew(async () =>
                {
                    string azure = "https://jrsimplemessagessignalr.azurewebsites.net/MessageHub";

                    HubConnection connection;
                    connection = new HubConnectionBuilder()
                        .WithUrl(azure)
                        .Build();

                    await connection.StartAsync();

                    await connection.SendAsync("SendMessage", input[0].GetPropertyValue<string>("key"), input[0]);

                });
            }
        }
    }
}
