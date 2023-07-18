using System.Text;
using System.Text.Json;
using TN_AI_NOTES_DEMO;

namespace TN_AI_NOTES_DEMO2
{
    internal class GPT : IArtificialIntelligence
    {
        private static readonly Lazy<GPT> lazyInstance = new Lazy<GPT>(() => new GPT());
        private readonly HttpClient _httpClient;
        private readonly string _engine;

        record CompletionResponse(Choice[] choices);
        record Choice(string text);

        private GPT()
        {
            _httpClient = new HttpClient();
            var apikey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apikey}");
            _engine = "text-davinci-003";
        }

        public static GPT Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        public async Task<string> Query(string prompt = "Once upon a time")
        {
            var request = new
            {
                prompt = prompt,
                temperature = 0.5,
                max_tokens = 500
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonContent = JsonSerializer.Serialize(request, jsonOptions);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://api.openai.com/v1/engines/{_engine}/completions", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<CompletionResponse>(jsonResponse);
            var generatedText = responseData?.choices?[0]?.text ?? string.Empty;

            return generatedText;
        }

    }
}
