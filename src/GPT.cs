using System.Text;
using System.Text.Json;
using TN_AI_NOTES_DEMO;

namespace TN_AI_NOTES_DEMO
{
    internal class GPT : IArtificialIntelligence
    {
        private static readonly Lazy<GPT> lazyInstance = new Lazy<GPT>(() => new GPT());
        private readonly HttpClient _httpClient;
        private Message _systemMessage = new Message("system", "you are an AI assistant");
        private List<Message> _conversation = new List<Message>();

        record CompletionResponse(Choice[] choices);
        record Choice(Message message);

        record Message(string role, string content);




        private GPT()
        {
            _httpClient = new HttpClient();
            var apikey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apikey}");
            _conversation.Add(_systemMessage);
        }

        public static GPT Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }
        public void SetSystemMessage(string systemMessage)
        {
            _systemMessage = new Message("system", systemMessage);
            _conversation = new List<Message>();
            _conversation.Add(_systemMessage);
        }
        public async Task<string> Query(string prompt = "Once upon a time")
        {
            _conversation.Add(new Message("user", prompt));

            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = _conversation,
                temperature = 1,
                max_tokens = 256,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonContent = JsonSerializer.Serialize(request, jsonOptions);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://api.openai.com/v1/chat/completions", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<CompletionResponse>(jsonResponse);
            var generatedText = responseData?.choices?[0]?.message?.content ?? string.Empty;

            // add it to conversation context!
            _conversation.Add(new Message("assistant",generatedText));

            return generatedText;
        }

    }
}
