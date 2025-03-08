using System.Text.Json.Serialization;

namespace ZenithWebHandler.Models.Responses
{
    public class CreatedBuild
    {
        [JsonPropertyName("link")]
        public string Link { get; set; }
    }
}
