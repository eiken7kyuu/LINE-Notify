using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace LineNotify
{
    public class NotifyResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class NotifyParameter
    {
        public string Message { get; set; }
        public string ImageUrl { get; set; }
    }

    public class LineNotifyClient
    {
        private readonly string _notifyUri = "https://notify-api.line.me/api/notify";
        private readonly HttpClient _client = new HttpClient();

        public LineNotifyClient(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<NotifyResult> CreateResult(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<NotifyResult>(jsonString, options);
        }

        public async Task<NotifyResult> NotifyAsync(string message)
        {
            var parameter = new NotifyParameter { Message = message };
            return await NotifyAsync(parameter);
        }

        public async Task<NotifyResult> NotifyAsync(NotifyParameter parameter)
        {
            var parameters = new Dictionary<string, string>
            {
                { "message", parameter.Message },
                { "imageThumbnail", parameter.ImageUrl },
                { "imageFullsize", parameter.ImageUrl },
                { "Content-Type", "application/x-www-form-urlencoded" }
            };

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(_notifyUri),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(parameters)
            };

            var response = await _client.SendAsync(requestMessage);
            return await CreateResult(response);
        }
    }
}