using Azure.Messaging.ServiceBus;

namespace ServiceBusQueue
{
    internal class Sender
    {
        internal static async Task StartSender(ServiceBusClient client, string queueName)
        {
            var sender = client.CreateSender(queueName);
            using var serviceBusMessageBatch = await sender.CreateMessageBatchAsync();
            for (int i = 0; i < 3; i++) 
            {
                var message = new ServiceBusMessage($"Message {i}");
                if (!serviceBusMessageBatch.TryAddMessage(message))
                    throw new Exception("Unable to add message");
            }

            try
            {
                await sender.SendMessagesAsync(serviceBusMessageBatch);
                Console.Out.WriteLine("Messages sent");
            }
            finally 
            {
                await sender.DisposeAsync();
            }
            Console.ReadKey();
        }
    }
}
