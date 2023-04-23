using TelegramAPI;

namespace TelegramAuth;

public class Program
{
    static async Task Main(string[] args)
    {
        TelegramClient client = new TelegramClient(
            clientName: Guid.NewGuid().ToString(),
            apiHash: "608684bc8defbee867c2cfc12b798c9f",
            apiId: 21688496);

        Console.WriteLine(client);

        await Start(client);

        client.Dispose();
    }

    static async Task Start(TelegramClient client)
    {
        await client.Init();
    }
}