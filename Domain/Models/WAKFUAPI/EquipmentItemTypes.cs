using Newtonsoft.Json;
using WakfuItemsPlayground.Enums;

namespace Domain.Models.WAKFUAPI
{
    public class EquipmentDefinition
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("equipmentPositions")]
        public List<string> EquipmentPositions { get; set; }

        [JsonProperty("equipmentDisabledPositions")]
        public List<string> EquipmentDisabledPositions { get; set; }

        [JsonProperty("isRecyclable")]
        public bool IsRecyclable { get; set; }

        [JsonProperty("isVisibleInAnimation")]
        public bool IsVisibleInAnimation { get; set; }
    }

    public class EquipmentTitle
    {
        [JsonProperty("fr")]
        public string French { get; set; }

        [JsonProperty("en")]
        public string English { get; set; }

        [JsonProperty("es")]
        public string Spanish { get; set; }

        [JsonProperty("pt")]
        public string Portuguese { get; set; }
    }

    public class EquipmentItemTypes
    {
        [JsonProperty("definition")]
        public EquipmentDefinition Definition { get; set; }

        [JsonProperty("title")]
        public EquipmentTitle Title { get; set; }
        public ItemTypesEnum ItemTypeEnum { get; set; }
    }
}
