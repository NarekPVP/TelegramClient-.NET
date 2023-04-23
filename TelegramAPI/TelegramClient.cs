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
        public string? Phone { get; private set; }

        private bool disposed = false;

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

            //string botToken = "";
            //var url = $"https://api.telegram.org/bot{botToken}/auth/sendCode";
            //var data = $"phone_number={phone}&settings={{\"is_bot\":false,\"allow_flashcall\":false,\"current_number\":\"\",\"api_id\":{ApiId},\"api_hash\":\"{ApiHash}\",\"lang_code\":\"en\"}}";

            //using var client = new HttpClient();
            //var content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            //var response = await client.PostAsync(url, content);
            //var result = await response.Content.ReadAsStringAsync();

            //await Console.Out.WriteLineAsync(result);

            // Replace <api_id> and <api_hash> with your Telegram API ID and hash.
            var apiId = ApiId;
            var apiHash = ApiHash;

            // Replace <your_phone_number> with your actual phone number (including country code).
            var phoneNumber = phone;

            // Send a request to Telegram to authenticate your phone number and get the verification code.
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://my.telegram.org/auth/send_password?phone={phoneNumber}&api_id={apiId}&api_hash={apiHash}");

            if (response.IsSuccessStatusCode)
            {
                // Prompt the user to enter the verification code.
                Console.WriteLine("Enter the verification code you received from Telegram:");
                var verificationCode = Console.ReadLine();

                // Send a request to Telegram to verify the code and authenticate the phone number.
                response = await httpClient.GetAsync($"https://my.telegram.org/auth/login?phone={phoneNumber}&password={verificationCode}&api_id={apiId}&api_hash={apiHash}");

                if (response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync("Successfully logged in!");
                }
                else
                {
                    Console.WriteLine($"Phone number authentication failed. Reason: {response.ReasonPhrase}");
                }
            }
            else
            {
                Console.WriteLine($"Failed to authenticate phone number. Reason: {response.ReasonPhrase}");
            }
        }

        public async Task SendMessage(HttpClient client, string botToken, string chatId, string message)
        {
            await client.GetAsync($"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={message}");
        }

        public async Task Init()
        {
            if (ApiHash == null || ApiId == null) 
                return;

            using (var client = new HttpClient())
            {
                using var result = await client.GetAsync("https://jsonplaceholder.typicode.com/todos/2");
                Console.WriteLine("Status: " + result.StatusCode);
                await Console.Out.WriteLineAsync(await result.Content.ReadAsStringAsync());
                
                result.Dispose();
            }
        }

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