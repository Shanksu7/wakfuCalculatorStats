using Domain.Enums;
using Domain.Models.WAKFUAPI;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Web;
using WakfuItemsPlayground.Enums;
using ZenithWebHandler.Extensions;
using ZenithWebHandler.Models;
using ZenithWebHandler.Models.Responses;

namespace ZenithWebHandler.Handler
{
    public class ZenithHandler
    {
        public static async Task<string> Create(int lvl, string name)
        {
            var job = new CreateZenithBuildModel
            {
                Name = name,
                Level = lvl,
                JobId = 10,
                IsVisible = true,
                Flags = new List<string>()
            };

            var response = await client.CreateBuild(job);
            return response.Link;//6rrum
        }

        public static async Task<ZenithBuildInfo> GetBuild(string buildCode)
        {
            var response = await client.GetBuild(buildCode);
            return response;
        }

        public static async Task<ItemZenith> GetSingleItem(Item item)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);

            queryParams["name"] = item.Title.Es;
            queryParams["rarity[]"] = item.Definition.Item.BaseParameters.Rarity.ToString();
            queryParams["minLvl"] = (item.Definition.Item.Level-1).ToString();
            queryParams["maxLvl"] = item.Definition.Item.BaseParameters.ItemTypeId == 611 ? "50" : item.Definition.Item.Level.ToString();
            var _params = queryParams.ToString().Replace('+', ' ');
            var items = await client.GetItems(_params);
            var data = items.FirstOrDefault(x => x.IdEquipment.Value == item.Definition.Item.Id);
            if (data == null)
                throw new Exception($"Cannot find item {item.ToString()}");
            Console.WriteLine("Fetched in Zenith: " + item.Title.Es);
            return data;
        }

        public static async Task<HttpStatusCode> Add(ItemZenith item, int idBuild)
        {
            Console.WriteLine("Adding item " + item.ToString() + " to build " + idBuild);
            var json = JsonSerializer.Deserialize<AddItemZenith>("{\"equipment\":{\"id_equipment\":30703,\"gfx_id\":\"11030703\",\"level\":230,\"id_equipment_type\":518,\"id_rarity\":5,\"ap_cost\":6,\"mp_cost\":0,\"wp_cost\":0,\"min_range\":1,\"max_range\":1,\"name_equipment\":\"Epée de Valdain\",\"line_of_sight\":1,\"name_equipment_type\":\"Armes 1 Main\",\"image_equipment_type\":\"one_handed_weapon.png\",\"order\":13,\"name_rarity\":\"Relique\",\"image_rarity\":\"relic.png\",\"effects\":[{\"id_effect\":345639,\"name_effect\":\"Dommage [el6] : [#1]\",\"ui_position\":0,\"from_evolution\":-1,\"container_min_level\":0,\"container_max_level\":32767,\"is_critical\":0,\"is_use_effect\":1,\"pivot\":{\"foreign_type\":30703,\"id_effect\":345639},\"translations\":[{\"id\":4667314,\"table_name\":\"effects\",\"column_name\":\"name_effect\",\"foreign_key\":345639,\"locale\":\"es\",\"value\":\"Daños [el6]: [#1]\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"values\":[{\"id_spell_effect_value\":1260891,\"offset\":0,\"damage\":6,\"ratio\":0.62900000810623,\"id_stats\":1083,\"id_effect\":345639,\"will_calculate_damage\":1,\"will_calculate_heal\":0,\"base\":null,\"evolution\":null,\"random_number\":0,\"statistics\":null}],\"inner_states\":[]},{\"id_effect\":345640,\"name_effect\":\"Dommage [el6] : [#1]\",\"ui_position\":7,\"from_evolution\":0,\"container_min_level\":0,\"container_max_level\":32767,\"is_critical\":1,\"is_use_effect\":1,\"pivot\":{\"foreign_type\":30703,\"id_effect\":345640},\"translations\":[{\"id\":4667335,\"table_name\":\"effects\",\"column_name\":\"name_effect\",\"foreign_key\":345640,\"locale\":\"es\",\"value\":\"Daños [el6]: [#1]\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"values\":[{\"id_spell_effect_value\":1260898,\"offset\":0,\"damage\":7.5,\"ratio\":0.78624999523163,\"id_stats\":1083,\"id_effect\":345640,\"will_calculate_damage\":1,\"will_calculate_heal\":0,\"base\":null,\"evolution\":null,\"random_number\":0,\"statistics\":null}],\"inner_states\":[]},{\"id_effect\":368499,\"name_effect\":\"[#charac AP] [#1] PA\",\"ui_position\":1,\"from_evolution\":0,\"container_min_level\":0,\"container_max_level\":-32513,\"is_critical\":0,\"is_use_effect\":0,\"pivot\":{\"foreign_type\":30703,\"id_effect\":368499},\"translations\":[{\"id\":4667317,\"table_name\":\"effects\",\"column_name\":\"name_effect\",\"foreign_key\":368499,\"locale\":\"es\",\"value\":\"[#charac AP] [#1] PA\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"values\":[{\"id_spell_effect_value\":1260892,\"offset\":0,\"damage\":2,\"ratio\":0,\"id_stats\":31,\"id_effect\":368499,\"will_calculate_damage\":0,\"will_calculate_heal\":0,\"base\":null,\"evolution\":null,\"random_number\":0,\"statistics\":{\"id_stats\":31,\"name_stats\":\"PA\",\"image_stats\":\"pa.webp\",\"base_stats\":6,\"order_stats\":2,\"id_stats_type\":1,\"order\":1,\"element\":null,\"search_display\":1,\"related_to\":null,\"related_multiplier\":null,\"parent_statistic_id\":null,\"is_negative\":null}}],\"inner_states\":[]},{\"id_effect\":368500,\"name_effect\":\"[#charac WP] [#1] PW\",\"ui_position\":2,\"from_evolution\":0,\"container_min_level\":0,\"container_max_level\":-32513,\"is_critical\":0,\"is_use_effect\":0,\"pivot\":{\"foreign_type\":30703,\"id_effect\":368500},\"translations\":[{\"id\":4667320,\"table_name\":\"effects\",\"column_name\":\"name_effect\",\"foreign_key\":368500,\"locale\":\"es\",\"value\":\"[#charac WP] [#1] PW\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"values\":[{\"id_spell_effect_value\":1260893,\"offset\":0,\"damage\":1,\"ratio\":0,\"id_stats\":191,\"id_effect\":368500,\"will_calculate_damage\":0,\"will_calculate_heal\":0,\"base\":null,\"evolution\":null,\"random_number\":0,\"statistics\":{\"id_stats\":191,\"name_stats\":\"PW\",\"image_stats\":\"pw.webp\",\"base_stats\":6,\"order_stats\":4,\"id_stats_type\":1,\"order\":3,\"element\":null,\"search_display\":1,\"related_to\":191191,\"related_multiplier\":75,\"parent_statistic_id\":null,\"is_negative\":null}}],\"inner_states\":[]},{\"id_effect\":368501,\"name_effect\":\"[#charac HP] [#1] PV\",\"ui_position\":3,\"from_evolution\":0,\"container_min_level\":0,\"container_max_level\":32767,\"is_critical\":0,\"is_use_effect\":0,\"pivot\":{\"foreign_type\":30703,\"id_effect\":368501},\"translations\":[{\"id\":4667323,\"table_name\":\"effects\",\"column_name\":\"name_effect\",\"foreign_key\":368501,\"locale\":\"es\",\"value\":\"[#charac HP] [#1] PdV\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"values\":[{\"id_spell_effect_value\":1260894,\"offset\":0,\"damage\":336,\"ratio\":0,\"id_stats\":20,\"id_effect\":368501,\"will_calculate_damage\":0,\"will_calculate_heal\":0,\"base\":null,\"evolution\":null,\"random_number\":0,\"statistics\":{\"id_stats\":20,\"name_stats\":\"PdV\",\"image_stats\":\"health_point.webp\",\"base_stats\":0,\"order_stats\":1,\"id_stats_type\":1,\"order\":6,\"element\":null,\"search_display\":1,\"related_to\":null,\"related_multiplier\":null,\"parent_statistic_id\":null,\"is_negative\":null}}],\"inner_states\":[]},{\"id_effect\":368502,\"name_effect\":\"[#charac RANGED_DMG] [#1] Maîtrise Distance\",\"ui_position\":4,\"from_evolution\":0,\"container_min_level\":0,\"container_max_level\":-32513,\"is_critical\":0,\"is_use_effect\":0,\"pivot\":{\"foreign_type\":30703,\"id_effect\":368502},\"translations\":[{\"id\":4667326,\"table_name\":\"effects\",\"column_name\":\"name_effect\",\"foreign_key\":368502,\"locale\":\"es\",\"value\":\"[#charac RANGED_DMG] [#1] Dominio distancia\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"values\":[{\"id_spell_effect_value\":1260895,\"offset\":0,\"damage\":610,\"ratio\":0,\"id_stats\":1053,\"id_effect\":368502,\"will_calculate_damage\":0,\"will_calculate_heal\":0,\"base\":null,\"evolution\":null,\"random_number\":0,\"statistics\":{\"id_stats\":1053,\"name_stats\":\"Maîtrise Distance\",\"image_stats\":\"range_mastery.webp\",\"base_stats\":0,\"order_stats\":6,\"id_stats_type\":5,\"order\":24,\"element\":null,\"search_display\":1,\"related_to\":null,\"related_multiplier\":null,\"parent_statistic_id\":null,\"is_negative\":null}}],\"inner_states\":[]},{\"id_effect\":368503,\"name_effect\":\"[#charac FEROCITY] [#1]% Coup critique\",\"ui_position\":5,\"from_evolution\":0,\"container_min_level\":0,\"container_max_level\":32767,\"is_critical\":0,\"is_use_effect\":0,\"pivot\":{\"foreign_type\":30703,\"id_effect\":368503},\"translations\":[{\"id\":4667329,\"table_name\":\"effects\",\"column_name\":\"name_effect\",\"foreign_key\":368503,\"locale\":\"es\",\"value\":\"[#charac FEROCITY] [#1]% de golpe crítico\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"values\":[{\"id_spell_effect_value\":1260896,\"offset\":0,\"damage\":3,\"ratio\":0,\"id_stats\":150,\"id_effect\":368503,\"will_calculate_damage\":0,\"will_calculate_heal\":0,\"base\":null,\"evolution\":null,\"random_number\":0,\"statistics\":{\"id_stats\":150,\"name_stats\":\"Coup Critique\",\"image_stats\":\"criticalhit.webp\",\"base_stats\":3,\"order_stats\":3,\"id_stats_type\":4,\"order\":12,\"element\":null,\"search_display\":1,\"related_to\":null,\"related_multiplier\":null,\"parent_statistic_id\":null,\"is_negative\":null}}],\"inner_states\":[]},{\"id_effect\":368504,\"name_effect\":\"[#charac RES_IN_PERCENT] [#1] Résistance Élémentaire\",\"ui_position\":6,\"from_evolution\":0,\"container_min_level\":0,\"container_max_level\":-32513,\"is_critical\":0,\"is_use_effect\":0,\"pivot\":{\"foreign_type\":30703,\"id_effect\":368504},\"translations\":[{\"id\":4667332,\"table_name\":\"effects\",\"column_name\":\"name_effect\",\"foreign_key\":368504,\"locale\":\"es\",\"value\":\"[#charac RES_IN_PERCENT] [#1] Resistencia elemental\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"values\":[{\"id_spell_effect_value\":1260897,\"offset\":0,\"damage\":10,\"ratio\":0,\"id_stats\":80,\"id_effect\":368504,\"will_calculate_damage\":0,\"will_calculate_heal\":0,\"base\":null,\"evolution\":null,\"random_number\":0,\"statistics\":{\"id_stats\":80,\"name_stats\":\"Résistance élémentaire\",\"image_stats\":\"element_resistance.webp\",\"base_stats\":0,\"order_stats\":0,\"id_stats_type\":null,\"order\":33,\"element\":null,\"search_display\":1,\"related_to\":null,\"related_multiplier\":null,\"parent_statistic_id\":null,\"is_negative\":null}}],\"inner_states\":[]}],\"criterias\":[],\"translations\":[{\"id\":4667311,\"table_name\":\"equipment\",\"column_name\":\"name_equipment\",\"foreign_key\":30703,\"locale\":\"es\",\"value\":\"Espada de Valdain\",\"created_at\":\"2025-02-12T13:43:47.000000Z\",\"updated_at\":\"2025-02-12T13:43:47.000000Z\"}],\"metadata\":{\"side\":540}},\"id_build\":1488607}");
            var result = await client.AddItem(new AddItemZenith() { Equipment = item, IdBuild = idBuild });
            return result;
        }

        static HttpClient client => new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.All });

    }
}

