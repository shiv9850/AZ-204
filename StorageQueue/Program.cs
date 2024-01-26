using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace StorageQueue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "";
            var queueName = "";

            var client = new QueueClient(connectionString, queueName);
            client.CreateIfNotExists();
            client.SendMessage("Test Message");


            //Peek message
            client.PeekMessage();



            //update message
            QueueMessage[] message = client.ReceiveMessages();

            // Update the message contents
            client.UpdateMessage(message[0].MessageId,
                    message[0].PopReceipt,
                    "Updated contents",
                    TimeSpan.FromSeconds(60.0)  // Make it invisible for another 60 seconds
                );


            //length of message
            QueueProperties properties = client.GetProperties();

            // Retrieve the cached approximate message count.
            int cachedMessagesCount = properties.ApproximateMessagesCount;

            // Display number of messages.
            Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");


            //Dequeu
            QueueMessage[] retrievedMessage = client.ReceiveMessages();

            // Process (i.e. print) the message in less than 30 seconds
            Console.WriteLine($"Dequeued message: '{retrievedMessage[0].Body}'");

            // Delete the message
            client.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);


            // Delete the queue
            client.Delete();
        }
    }
}
