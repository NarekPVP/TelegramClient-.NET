using System.Net.Http;
using System.Text;

namespace TelegramAPI
{
    public class TelegramClient : IDisposable
    {
        /*
         curl
        -X POST "https://api.telegram.org/bot<your_bot_token>/auth/sendCode"
        -d "phone_number=<your_phone_number>&amp;settings={\"is_bot\":false,\"allow_flashcall\":false,\"current_number\":\"\",\"api_id\":<your_api_id>,\"api_hash\":\"<your_api_hash>\",\"lang_code\":\"en\"}"
         */

        public string ClientName { get; private set; }
        public string? ApiHash { get; private set; }
        public int? ApiId { get; private set; }
        public string? BotToken { get; private set; }
        public string? Phone { get; private set; }

        private bool disposed = false;

        private const string ApiUrl = "https://my.telegram.org";

        public TelegramClient(string clientName)
        {
            ClientName = clientName;
        }

        public TelegramClient(string clientName, string apiHash, int apiId)
        {
            ClientName = clientName;
            ApiHash = apiHash;
            ApiId = apiId;
        }

        public TelegramClient(string clientName, string apiHash, string botToken, int apiId)
        {
            ClientName = clientName;
            ApiHash = apiHash;
            BotToken = botToken;
            ApiId = apiId;
        }

        public TelegramClient(string clientName, string apiHash, int apiId, string phone)
        {
            ClientName = clientName;
            ApiHash = apiHash;
            ApiId = apiId;
            Phone = phone;
        }

        public async Task Start(string phone)
        {
            if (ApiHash == null || ApiId == null)
                return;

            Phone = phone;

            // Send a request to Telegram to authenticate your phone number and get the verification code.
            using var client = new HttpClient();
            var response = await client.GetAsync($"{ApiUrl}/auth/send_password?phone={phone}&api_id={ApiId}&api_hash={ApiHash}");

            if (response.IsSuccessStatusCode)
            {
                // Prompt the user to enter the verification code.
                await Console.Out.WriteAsync("Enter the verification code you received from Telegram: ");
                var verificationCode = Console.ReadLine();

                // Send a request to Telegram to verify the code and authenticate the phone number.
                response = await client.GetAsync($"{ApiUrl}/auth/login?phone={phone}&password={verificationCode}&api_id={ApiId}&api_hash={ApiHash}");

                if (response.IsSuccessStatusCode)
                    await Console.Out.WriteLineAsync("Successfully logged in!");
                else
                    await Console.Out.WriteLineAsync($"Phone number authentication failed. Reason: {response.ReasonPhrase}");
            }
            else
                await Console.Out.WriteLineAsync($"Failed to authenticate phone number. Reason: {response.ReasonPhrase}");
        }

        public async Task<HttpResponseMessage> SendMessage(HttpClient client, string botToken, string chatId, string message)
            => await client.GetAsync($"{ApiUrl}/bot{botToken}/sendMessage?chat_id={chatId}&text={message}");

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TelegramClient()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            // ...
        }

        public override string ToString() => $"Client: {ClientName}\nApi Hash: {ApiHash}\nApi Id: {ApiId}";
    }
}