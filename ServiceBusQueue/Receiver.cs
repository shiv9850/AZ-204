using Azure.Messaging.ServiceBus;

namespace ServiceBusQueue
{
    internal class Receiver
    {
        internal static async Task StartReceiver(ServiceBusClient client, string queueName)
        {
            var processer = client.CreateProcessor(queueName);

            try
            {
                processer.ProcessMessageAsync += Processer_ProcessMessageAsync;
                processer.ProcessErrorAsync += Processer_ProcessErrorAsync;
                await processer.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processer.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
			finally 
            {
                await processer.DisposeAsync();  
                await client.DisposeAsync();
            }
        }

        private static Task Processer_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private static async  Task Processer_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            string body = arg.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. messages is deleted from the queue. 
            await arg.CompleteMessageAsync(arg.Message);
        }
    }
}
