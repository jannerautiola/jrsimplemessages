using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace SignalRTester
{
    class Program
    {

        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.WriteLine("End");
            Console.ReadKey();
        }



        static async Task MainAsync()
        {
            string local = "https://localhost:44372/MessageHub";
            string azure = "https://jrsimplemessagessignalr.azurewebsites.net/MessageHub";
            Tester t = new Tester(azure);
            await t.Run();
        }
    }

    public class Tester
    {
        HubConnection connection;
        public Tester(string url)
        {
            connection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();
        }

        private bool Send = false;

        private string GroupKey = "";

        public async Task Run()
        {
            Console.Write("Send test messages (y/n): ");
            if (Console.ReadLine() == "y")
                Send = true;
            Console.Write("Group key: ");
            GroupKey = Console.ReadLine();

            await connection.StartAsync();

            connection.On<Message>("ReceiveMessage", ReceiveMessage);
            connection.Closed += Connection_Closed;

            await connection.SendAsync("Register", GroupKey);

            Console.WriteLine("Waiting...");

            while (!Console.KeyAvailable)
            {
                await Task.Delay(1000);

                if (Send)
                {
                    string key = GroupKey;

                    var dic = new Dictionary<string, string>();
                    dic.Add("ip", "127.0.0.1");
                    dic.Add("osoite", "sama");

                    Message message = new Message()
                    {
                        id = key + "_" + Guid.NewGuid().ToString(),
                        key = key,
                        timestamp = DateTime.UtcNow,
                        values = dic
                    };

                    await connection.SendAsync("SendMessage", key, message);
                }
            }
        }

        private void ReceiveMessage(Message obj)
        {
            Console.WriteLine("MessageReceived");
            Console.WriteLine($"MessageReceived: {obj.id}");
            Console.WriteLine($"MessageReceived: {obj.key}");
            Console.WriteLine($"MessageReceived: {obj.timestamp}");
            foreach (var item in obj.values)
            {
                Console.WriteLine($"{item.Key}:{item.Value}");
            }
        }

        private Task Connection_Closed(Exception arg)
        {
            Console.WriteLine("Connection_Closed");
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }
    }

    public class Message
    {
        public string id { get; set; }
        public string key { get; set; }
        public DateTime timestamp { get; set; }
        public Dictionary<string, string> values { get; set; }
    }
}
