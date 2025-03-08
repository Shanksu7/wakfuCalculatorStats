using System.Text.Json.Serialization;

namespace ZenithWebHandler.Models.Responses
{
    public class ZenithBuildInfo
    {
        [JsonPropertyName("id_build")]
        public int IdBuild { get; set; }

        [JsonPropertyName("name_build")]
        public string NameBuild { get; set; }

        [JsonPropertyName("date_build")]
        public string DateBuild { get; set; }

        [JsonPropertyName("level_build")]
        public int LevelBuild { get; set; }

        [JsonPropertyName("id_job")]
        public int IdJob { get; set; }

        [JsonPropertyName("link_build")]
        public string LinkBuild { get; set; }

        [JsonPropertyName("private")]
        public int Private { get; set; }

        [JsonPropertyName("id_user")]
        public int? IdUser { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; }

        [JsonPropertyName("name_job")]
        public string NameJob { get; set; }

        [JsonPropertyName("image_job")]
        public string ImageJob { get; set; }

        [JsonPropertyName("display")]
        public int Display { get; set; }

        [JsonPropertyName("isFavourite")]
        public bool IsFavourite { get; set; }

        [JsonPropertyName("pictos")]
        public object[] Pictos { get; set; }
    }

}
