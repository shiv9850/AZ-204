using StackExchange.Redis;

namespace RedisCache
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "REDIS_CONNECTION_STRING";

            using (var cache = ConnectionMultiplexer.Connect(connectionString))
            {
                IDatabase db =   cache.GetDatabase();

                //ping-pong
                var result = await db.ExecuteAsync("ping");
                Console.WriteLine($"PING : {result}");

                var setValue = await db.StringSetAsync("TestKey", "TestValue");
                Console.WriteLine($"SET: {setValue}");


                var getValue = await db.StringGetAsync("TestKey");
                Console.WriteLine($"GET: {getValue}");
            }
        }
    }
}
