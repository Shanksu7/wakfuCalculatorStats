using System.Text.Json.Serialization;

namespace ZenithWebHandler.Models.Responses
{
    public class ItemZenith
    {
        [JsonPropertyName("id_equipment")]
        public int? IdEquipment { get; set; }

        [JsonPropertyName("gfx_id")]
        public string GfxId { get; set; }

        [JsonPropertyName("level")]
        public int? Level { get; set; }

        [JsonPropertyName("id_equipment_type")]
        public int? IdEquipmentType { get; set; }

        [JsonPropertyName("id_rarity")]
        public int? IdRarity { get; set; }

        [JsonPropertyName("ap_cost")]
        public int? ApCost { get; set; }

        [JsonPropertyName("mp_cost")]
        public int? MpCost { get; set; }

        [JsonPropertyName("wp_cost")]
        public int? WpCost { get; set; }

        [JsonPropertyName("min_range")]
        public int? MinRange { get; set; }

        [JsonPropertyName("max_range")]
        public int? MaxRange { get; set; }

        [JsonPropertyName("name_equipment")]
        public string NameEquipment { get; set; }

        [JsonPropertyName("line_of_sight")]
        public int? LineOfSight { get; set; }

        [JsonPropertyName("name_equipment_type")]
        public string NameEquipmentType { get; set; }

        [JsonPropertyName("image_equipment_type")]
        public string ImageEquipmentType { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("name_rarity")]
        public string NameRarity { get; set; }

        [JsonPropertyName("image_rarity")]
        public string ImageRarity { get; set; }

        [JsonPropertyName("effects")]
        public List<Effect> Effects { get; set; }

        [JsonPropertyName("criterias")]
        public List<object> Criterias { get; set; }

        [JsonPropertyName("translations")]
        public List<Translation> Translations { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        public override string ToString() => $"[{IdEquipment}] {Translations[0].Value}";
    }

    public class Effect
    {
        [JsonPropertyName("id_effect")]
        public int? IdEffect { get; set; }

        [JsonPropertyName("name_effect")]
        public string NameEffect { get; set; }

        [JsonPropertyName("ui_position")]
        public int? UiPosition { get; set; }

        [JsonPropertyName("from_evolution")]
        public int? FromEvolution { get; set; }

        [JsonPropertyName("container_min_level")]
        public int? ContainerMinLevel { get; set; }

        [JsonPropertyName("container_max_level")]
        public int? ContainerMaxLevel { get; set; }

        [JsonPropertyName("is_critical")]
        public int? IsCritical { get; set; }

        [JsonPropertyName("is_use_effect")]
        public int? IsUseEffect { get; set; }

        [JsonPropertyName("pivot")]
        public Pivot Pivot { get; set; }

        [JsonPropertyName("translations")]
        public List<Translation> Translations { get; set; }

        [JsonPropertyName("values")]
        public List<Value> Values { get; set; }

        [JsonPropertyName("inner_states")]
        public List<object> InnerStates { get; set; }
    }

    public class Pivot
    {
        [JsonPropertyName("foreign_type")]
        public int? ForeignType { get; set; }

        [JsonPropertyName("id_effect")]
        public int? IdEffect { get; set; }
    }

    public class Statistics
    {
        [JsonPropertyName("id_stats")]
        public int? IdStats { get; set; }

        [JsonPropertyName("name_stats")]
        public string NameStats { get; set; }

        [JsonPropertyName("image_stats")]
        public string ImageStats { get; set; }

        [JsonPropertyName("base_stats")]
        public int? BaseStats { get; set; }

        [JsonPropertyName("order_stats")]
        public int? OrderStats { get; set; }

        [JsonPropertyName("id_stats_type")]
        public int? IdStatsType { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("element")]
        public object Element { get; set; }

        [JsonPropertyName("search_display")]
        public int? SearchDisplay { get; set; }

        [JsonPropertyName("related_to")]
        public int? RelatedTo { get; set; }

        [JsonPropertyName("related_multiplier")]
        public int? RelatedMultiplier { get; set; }

        [JsonPropertyName("parent_statistic_id")]
        public object ParentStatisticId { get; set; }

        [JsonPropertyName("is_negative")]
        public object IsNegative { get; set; }
    }

    public class Translation
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("table_name")]
        public string TableName { get; set; }

        [JsonPropertyName("column_name")]
        public string ColumnName { get; set; }

        [JsonPropertyName("foreign_key")]
        public int? ForeignKey { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("id_spell_effect_value")]
        public int? IdSpellEffectValue { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        [JsonPropertyName("damage")]
        public double? Damage { get; set; }

        [JsonPropertyName("ratio")]
        public double? Ratio { get; set; }

        [JsonPropertyName("id_stats")]
        public int? IdStats { get; set; }

        [JsonPropertyName("id_effect")]
        public int? IdEffect { get; set; }

        [JsonPropertyName("will_calculate_damage")]
        public int? WillCalculateDamage { get; set; }

        [JsonPropertyName("will_calculate_heal")]
        public int? WillCalculateHeal { get; set; }

        [JsonPropertyName("base")]
        public object Base { get; set; }

        [JsonPropertyName("evolution")]
        public object Evolution { get; set; }

        [JsonPropertyName("random_number")]
        public int? RandomNumber { get; set; }

        [JsonPropertyName("statistics")]
        public Statistics Statistics { get; set; }

        [JsonPropertyName("elements")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Elements> Elements { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("side")]
        public int Side { get; set; }
    }

    public class Elements
    {
        [JsonPropertyName("id_element")]
        public int IdElement { get; set; }
        [JsonPropertyName("id_inner_stats")]
        public int IdInnerStats { get; set; }
        [JsonPropertyName("image_element")]
        public string ImageElement { get; set; }
    }


}
