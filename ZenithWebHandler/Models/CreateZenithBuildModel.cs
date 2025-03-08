using System.Text.Json.Serialization;

namespace ZenithWebHandler.Models
{
    public class CreateZenithBuildModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("id_job")]
        public int JobId { get; set; }

        [JsonPropertyName("is_visible")]
        public bool IsVisible { get; set; }

        [JsonPropertyName("flags")]
        public List<string> Flags { get; set; }
    }
}
