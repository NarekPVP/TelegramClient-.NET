namespace TelegramAPI
{
    public class TelegramClient : IDisposable
    {
        /*
         curl
        -X POST "https://api.telegram.org/bot<your_bot_token>/auth/sendCode"
        -d "phone_number=<your_phone_number>&amp;settings={\"is_bot\":false,\"allow_flashcall\":false,\"current_number\":\"\",\"api_id\":<your_api_id>,\"api_hash\":\"<your_api_hash>\",\"lang_code\":\"en\"}"
         */

        public string ClientName { get; set; }
        public string? ApiHash { get; set; }
        public int? ApiId { get; set; }

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