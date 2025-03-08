using System.Net;
using System.Text;
using System.Text.Json;
using ZenithWebHandler.Models;
using ZenithWebHandler.Models.Responses;

namespace ZenithWebHandler.Extensions
{
    public static class HttpZenithExtensions
    {
        const string apiUrl = "https://api.zenithwakfu.com/builder/api/";


        public static async Task<CreatedBuild> CreateBuild(this HttpClient client, CreateZenithBuildModel model)
        => await client.Post<CreateZenithBuildModel, CreatedBuild>("create", model);

        public static async Task<ZenithBuildInfo> GetBuild(this HttpClient client, string buildCode)
        => await client.Get<ZenithBuildInfo>($"infos/build/{buildCode}");

        public static async Task<List<ItemZenith>> GetItems(this HttpClient client, string queryParams)
        => await client.Get<List<ItemZenith>>("equipment?" + queryParams);

        public static async Task<HttpStatusCode> AddItem(this HttpClient client, AddItemZenith addItem)
        => await client.Post($"equipment/add", addItem);

        public static string GetZenithLink(this string codeBuild) => "https://www.zenithwakfu.com/builder/" + codeBuild;

        static void AddHeaders(this HttpRequestMessage request)
        {
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:136.0) Gecko/20100101 Firefox/136.0");
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("X-XSRF-TOKEN", "");
            request.Headers.Add("Origin", "https://www.zenithwakfu.com");
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Referer", "https://www.zenithwakfu.com/");
            request.Headers.Add("Cookie", "lang=es;");
            request.Headers.Add("Sec-Fetch-Dest", "empty");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("Sec-Fetch-Site", "same-site");
        }

        static async Task<TResponse> Post<TContent, TResponse>(this HttpClient client, string endpoint, TContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl + endpoint);

            request.AddHeaders();
            string jsonContent = JsonSerializer.Serialize(content);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseBody);
        }

        static async Task<HttpStatusCode> Post<TContent>(this HttpClient client, string endpoint, TContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl + endpoint);

            request.AddHeaders();

            var options = new JsonSerializerOptions
            {
                //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                //WriteIndented = true // Optional: Makes JSON output more readable
            };

            string jsonContent = JsonSerializer.Serialize(content, options);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        static async Task<T> Get<T>(this HttpClient client, string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl + endpoint);

            request.AddHeaders();
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(responseBody);
            return result;
        }
    }
}
