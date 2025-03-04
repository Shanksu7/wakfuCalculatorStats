using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI
{
    using System;
    using System.Collections.Generic;
    using Domain.Models.Stats;
    using Domain.Models.WAKFUAPI.Parsed;
    using Newtonsoft.Json;
    using WakfuItemsPlayground.Enums;

    public class Item
    {
        [JsonProperty("definition")]
        public ItemDefinition Definition { get; set; }

        [JsonProperty("title")]
        public Lang Title { get; set; }

        [JsonProperty("description")]
        public Lang Description { get; set; }
        public override string ToString()
        {
            return $"[{Definition.Item.Id}]" + $"[{Definition.Item.Level}]" + Title.Es;
        }
        public string Url
        {
            get
            {
                var text = string.Empty;
                var urlArmor = "https://www.wakfu.com/es/mmorpg/enciclopedia/armaduras/{0}-{1}";
                var urlwep = "https://www.wakfu.com/es/mmorpg/enciclopedia/armas/{0}-{1}";

                text += string.Format(urlArmor, Definition.Item.Id, Title.Es.Replace(" ", "-")) + "\n";
                return text;
            }
        }
    }

    public class ItemDefinition
    {
        [JsonProperty("item")]
        public ItemInfo Item { get; set; }

        [JsonProperty("useEffects")]
        public List<object> UseEffects { get; set; }

        [JsonProperty("useCriticalEffects")]
        public List<object> UseCriticalEffects { get; set; }

        [JsonProperty("equipEffects")]
        public List<EquipEffect> EquipEffects { get; set; }
        public List<EquipEffectParsed> EquipEffectsParsed { get; set; }
        public StatsCollection StatsCollection { get; set; }
    }

    public class ItemInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("baseParameters")]
        public BaseParameters BaseParameters { get; set; }

        [JsonProperty("useParameters")]
        public UseParameters UseParameters { get; set; }

        [JsonProperty("graphicParameters")]
        public GraphicParameters GraphicParameters { get; set; }

        [JsonProperty("properties")]
        public List<object> Properties { get; set; }
    }

    public class BaseParameters
    {
        [JsonProperty("itemTypeId")]
        public int ItemTypeId { get; set; }

        [JsonProperty("itemSetId")]
        public int ItemSetId { get; set; }

        [JsonProperty("rarity")]
        public int Rarity { get; set; }

        [JsonProperty("bindType")]
        public int BindType { get; set; }

        [JsonProperty("minimumShardSlotNumber")]
        public int MinimumShardSlotNumber { get; set; }

        [JsonProperty("maximumShardSlotNumber")]
        public int MaximumShardSlotNumber { get; set; }
        public ItemTypesEnum ItemTypeEnum { get; set; }
    }

    public class UseParameters
    {
        [JsonProperty("useCostAp")]
        public int UseCostAp { get; set; }

        [JsonProperty("useCostMp")]
        public int UseCostMp { get; set; }

        [JsonProperty("useCostWp")]
        public int UseCostWp { get; set; }

        [JsonProperty("useRangeMin")]
        public int UseRangeMin { get; set; }

        [JsonProperty("useRangeMax")]
        public int UseRangeMax { get; set; }

        [JsonProperty("useTestFreeCell")]
        public bool UseTestFreeCell { get; set; }

        [JsonProperty("useTestLos")]
        public bool UseTestLos { get; set; }

        [JsonProperty("useTestOnlyLine")]
        public bool UseTestOnlyLine { get; set; }

        [JsonProperty("useTestNoBorderCell")]
        public bool UseTestNoBorderCell { get; set; }

        [JsonProperty("useWorldTarget")]
        public int UseWorldTarget { get; set; }
    }

    public class GraphicParameters
    {
        [JsonProperty("gfxId")]
        public int GfxId { get; set; }

        [JsonProperty("femaleGfxId")]
        public int FemaleGfxId { get; set; }
    }

    public class EquipEffect
    {
        [JsonProperty("effect")]
        public Effect Effect { get; set; }
    }

    public class Effect
    {
        [JsonProperty("definition")]
        public EffectDefinition Definition { get; set; }
    }

    public class EffectDefinition
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("actionId")]
        public int ActionId { get; set; }

        [JsonProperty("areaShape")]
        public int AreaShape { get; set; }

        [JsonProperty("areaSize")]
        public List<int> AreaSize { get; set; }

        [JsonProperty("params")]
        public List<double> Params { get; set; }
    }

}
