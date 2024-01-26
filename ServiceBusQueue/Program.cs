using Azure.Messaging.ServiceBus;
namespace ServiceBusQueue
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "";
            var queueName = "";
            var client = new ServiceBusClient(connectionString);
            await Sender.StartSender(client, queueName);
            await Receiver.StartReceiver(client, queueName);

        }
    }
}
