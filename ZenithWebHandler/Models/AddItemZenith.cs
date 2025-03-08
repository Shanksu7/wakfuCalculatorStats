using System.Text.Json.Serialization;
using ZenithWebHandler.Models.Responses;

namespace ZenithWebHandler.Models
{
    public class AddItemZenith
    {
        [JsonPropertyName("equipment")]
        public ItemZenith Equipment { get; set; }

        [JsonPropertyName("id_build")]
        public int IdBuild { get; set; }
    }
}
