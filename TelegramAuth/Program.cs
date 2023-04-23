using TelegramAPI;

namespace TelegramAuth;

public class Program
{
    static async Task Main(string[] args)
    {
        string clientName = Guid.NewGuid().ToString();
        const string apiHash = "608684bc8defbee867c2cfc12b798c9f";
        const int apiId = 21688496;

        using(TelegramClient client = new TelegramClient(clientName: Guid.NewGuid().ToString(), apiHash, apiId))
        {
            Console.WriteLine(client);
            await Console.Out.WriteLineAsync();


            await Console.Out.WriteAsync("Enter phone number: ");
            string phone = Console.ReadLine();

            await client.Start(phone);
        }
    }
}