using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using SupportAI.Application.Interfaces;

namespace SupportAI.Infrastructure.AI
{
    public class GeminiChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiChatService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
        } 
        
        public async Task<string> AskAsync(string question, List<string> context)
        {
            var combinedContext = string.Join("\n\n", context);

            var prompt = $@"
                            You are customer support AI.
                            Answer only using the provided context.

                            If answer is not in context, say:
                            I don't know based on provided information.

                            Context: {combinedContext}

                            Question: {question}";

            var request = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                                            $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={_apiKey}",
                                            request);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(json);

            var doc = JsonDocument.Parse(json);

            var answer = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return answer;
        }
    }
}
