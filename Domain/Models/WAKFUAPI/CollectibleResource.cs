using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI
{
    public class CollectibleResource
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("skillId")]
        public int SkillId { get; set; }

        [JsonProperty("resourceId")]
        public int ResourceId { get; set; }

        [JsonProperty("resourceIndex")]
        public int ResourceIndex { get; set; }

        [JsonProperty("collectItemId")]
        public int CollectItemId { get; set; }

        [JsonProperty("resourceNextIndex")]
        public int ResourceNextIndex { get; set; }

        [JsonProperty("skillLevelRequired")]
        public int SkillLevelRequired { get; set; }

        [JsonProperty("simultaneousPlayer")]
        public int SimultaneousPlayer { get; set; }

        [JsonProperty("visualFeedbackId")]
        public int VisualFeedbackId { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("mruOrder")]
        public int MruOrder { get; set; }

        [JsonProperty("xpFactor")]
        public int XpFactor { get; set; }

        [JsonProperty("collectLootListId")]
        public int CollectLootListId { get; set; }

        [JsonProperty("collectConsumableItemId")]
        public int CollectConsumableItemId { get; set; }

        [JsonProperty("collectGfxId")]
        public int CollectGfxId { get; set; }

        [JsonProperty("displayInCraftDialog")]
        public bool DisplayInCraftDialog { get; set; }
    }
}
