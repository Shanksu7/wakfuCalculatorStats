using Newtonsoft.Json;

namespace Domain.Models.WAKFUAPI
{
    public class Blueprints
    {
        [JsonProperty("blueprintId")]
        public int BlueprintId { get; set; }

        [JsonProperty("recipeId")]
        public List<int> RecipeId { get; set; }
    }

}
