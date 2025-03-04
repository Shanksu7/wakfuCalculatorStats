using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
