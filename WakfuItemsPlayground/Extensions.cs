using Domain.Enums;
using Domain.Models.Stats;
using Domain.Models.Stats.Combat;
using Domain.Models.WAKFUAPI;
using System.Runtime.CompilerServices;
using WakfuItemsPlayground.Enums;

namespace WakfuItemsPlayground
{
    public static class Extensions
    {
        public static ItemTypesEnum[] GetOneHandTypes(this ItemTypesEnum _enum) => [ItemTypesEnum.AgujaUnaMano, ItemTypesEnum.ArmaUnaMano, ItemTypesEnum.BastonUnaMano, ItemTypesEnum.CartaUnaMano, ItemTypesEnum.EspadaUnaMano, ItemTypesEnum.VaritaUnaMano];
        public static ItemTypesEnum[] GetTwoHandedTypes(this ItemTypesEnum _enum) => [ItemTypesEnum.ArcoDosManos, ItemTypesEnum.ArmaDosManos, ItemTypesEnum.BastonDosManos, ItemTypesEnum.EspadaDosManos, ItemTypesEnum.HachaDosManos, ItemTypesEnum.MartilloDosManos, ItemTypesEnum.PalaDosManos];
        public static ItemTypesEnum[] GetSecondHandType(this ItemTypesEnum _enum) => [ItemTypesEnum.SegundaMano, ItemTypesEnum.DagaSegundaMano];
        public static string YesNo(this bool _bool) => _bool ? "Si" : "No";

        public static List<Item> Filter(this List<Item> items, int min, int max, ItemTypesEnum[] type = null, ItemRarity[] quality = null, Dictionary<StatsEnum, double> effects = null, bool? alwaysResis = null, List<StatsEnum> bannedStats = null, int[] excludedIds = null)
        {
            var result = items.Where(x =>
            (excludedIds == null || !excludedIds.Contains(x.Definition.Item.Id))
            && x.Definition.Item.Level >= min
            && x.Definition.Item.Level <= max
            && (type == null || type.Contains(x.Definition.Item.BaseParameters.ItemType.ItemTypeEnum))
            && (quality == null || quality.ToList().Contains((ItemRarity)x.Definition.Item.BaseParameters.Rarity))
            && (effects == null || x.CompareItem(effects))
            && (bannedStats == null || x.CompareItem(bannedStats))
            && (alwaysResis == null || x.HasResis())).ToList();

            //if (!result.Any()) throw new Exception($"NO ITEMS ON {string.Join(',', type)}");
            if (type != null)
                Console.WriteLine(string.Join(' ', type));
            if(quality != null)
                Console.WriteLine(string.Join(' ', quality));
            if(effects != null)
            {
                foreach(var (effect, value) in effects)
                    Console.WriteLine($"{effect.ToString()} : {value}");
            }
            Console.WriteLine();
            return result;
        }

        public static Item Get(this List<Item> items, int id) => items.FirstOrDefault(x => x.Definition.Item.Id == id);

        static bool CompareItem(this Item item, Dictionary<StatsEnum, double> dict)
        {
            if (item.Definition.Item.Id == 25664)
            {

            }
            foreach (var (stat, value) in dict)
            {
                if (!item.Definition.StatsCollection.Contains(stat, out var statvalue))
                {
                    if (value != 0)
                        return false;
                }
                if (statvalue < value) return false;
            }
            return true;
        }

        static bool CompareItem(this Item item, List<StatsEnum> stats)
        {
            foreach (var stat in stats)
            {
                if (item.Definition.StatsCollection.Contains(stat, out var statvalue))
                {
                    if (statvalue > 0)
                        return false;
                }
            }
            return true;
        }

        static bool HasResis(this Item item)
        {
            return 
                item.Definition.StatsCollection.Contains(StatsEnum.AIR_RESIST, out _) ||
                item.Definition.StatsCollection.Contains(StatsEnum.EARTH_RESIST, out _) ||
                item.Definition.StatsCollection.Contains(StatsEnum.WATER_RESIST, out _) ||
                item.Definition.StatsCollection.Contains(StatsEnum.FIRE_RESIST, out _);
        }

        public static Dictionary<Item, double> CalculateDamage(this List<Item> data, CalculateDamage damage, StatsCollection addedStats = null)
        {
            Dictionary<Item, double> result = new Dictionary<Item, double>();

            foreach (var item in data)
            {
                result.Add(item, item.CalculateDamage(damage, addedStats).Item2);
            }

            return result.OrderByDescending(x => x.Value).ToDictionary();
        }

        public static (Item, double) CalculateDamage(this Item data, CalculateDamage damage, StatsCollection addedStats = null)
        {
            return (data, data.Definition.StatsCollection.CalculateDamage(damage, addedStats));
        }

        public static Dictionary<Item, StatsCollection> SetStatsCollection(this List<Item> items)
        {
            var result = new Dictionary<Item, StatsCollection>();
            foreach (var item in items)
            {
                var (k, v) = item.SetStatsCollection();
                result.Add(k, v);
            }
            return result;
        }

        public static KeyValuePair<Item, StatsCollection> SetStatsCollection(this Item item, bool applyElecon = false, bool isHupper = false, bool applyAlternance = false, bool anatomy = false)
        {
            item.Definition.StatsCollection = item.GetStats(applyElecon, applyAlternance, isHupper, anatomy);
            return new KeyValuePair<Item, StatsCollection>(item, item.Definition.StatsCollection);
        }

        static StatsCollection GetStats(this Item item, bool applyElecon = false, bool applyAlternance = false, bool isHupper = false, bool anatomy = false)
        {
            StatsCollection stats = new StatsCollection();
            var dictionaryAdd = new Dictionary<int, StatsEnum[]>()
            {
                { 20, [StatsEnum.HP] },
                { 26, [StatsEnum.HEAL_DOMAIN] },
                { 31, [StatsEnum.AP] },
                { 39, [StatsEnum.RECEIVED_ARMOR] },
                { 41, [StatsEnum.MP] },
                { 71, [StatsEnum.REAR_RESISTANCE] },
                { 80, [StatsEnum.AIR_RESIST, StatsEnum.EARTH_RESIST, StatsEnum.FIRE_RESIST, StatsEnum.WATER_RESIST] },
                { 82, [StatsEnum.FIRE_RESIST] },
                { 83, [StatsEnum.WATER_RESIST] },
                { 84, [StatsEnum.EARTH_RESIST] },
                { 85, [StatsEnum.AIR_RESIST] },
                { 120, [StatsEnum.AIR_DOMAIN, StatsEnum.EARTH_DOMAIN, StatsEnum.FIRE_DOMAIN, StatsEnum.WATER_DOMAIN] },
                { 122, [StatsEnum.FIRE_DOMAIN] },
                { 123, [StatsEnum.EARTH_DOMAIN] },
                { 124, [StatsEnum.WATER_DOMAIN] },
                { 125, [StatsEnum.AIR_DOMAIN] },
                { 149, [StatsEnum.CRIT_DOMAIN] },
                { 150, [StatsEnum.CRIT_HIT] },
                { 160, [StatsEnum.RANGE] },
                { 162, [StatsEnum.PP] },
                { 166, [StatsEnum.WISDOM] },
                { 171, [StatsEnum.INI] },
                { 173, [StatsEnum.TACKLE] },
                { 175, [StatsEnum.DODGE] },
                { 177, [StatsEnum.WILL] },
                { 184, [StatsEnum.CONTROL] },
                { 180, [StatsEnum.REAR_DOMAIN] },
                { 191, [StatsEnum.WP] },
                { 875, [StatsEnum.BLOCK] },
                { 988, [StatsEnum.CRIT_RESISTANCE] },
                { 1052, [StatsEnum.MELE_DOMAIN] },
                { 1053, [StatsEnum.DISTANCE_DOMAIN] },
                { 1055, [StatsEnum.BERSERKER_DOMAIN] },
                { 1068, [StatsEnum.AIR_DOMAIN, StatsEnum.EARTH_DOMAIN, StatsEnum.FIRE_DOMAIN, StatsEnum.WATER_DOMAIN] },
                { 1069, [StatsEnum.AIR_RESIST, StatsEnum.EARTH_RESIST, StatsEnum.FIRE_RESIST, StatsEnum.WATER_RESIST] },
                { 2001, [StatsEnum.GATHERING_BONUS] },
                { 304, [StatsEnum.KSION_RING_BONUS] },
            };

            var dictionaryRemove = new Dictionary<int, StatsEnum[]>()
            {
                { 21, [StatsEnum.HP] },
                { 40, [StatsEnum.RECEIVED_ARMOR] },
                { 56, [StatsEnum.AP] },
                { 57, [StatsEnum.MP] },
                { 96, [StatsEnum.EARTH_RESIST] },
                { 97, [StatsEnum.FIRE_RESIST] },
                { 98, [StatsEnum.WATER_RESIST] },
                { 90, [StatsEnum.AIR_RESIST, StatsEnum.EARTH_RESIST, StatsEnum.FIRE_RESIST, StatsEnum.WATER_RESIST] },
                { 100, [StatsEnum.AIR_RESIST, StatsEnum.EARTH_RESIST, StatsEnum.FIRE_RESIST, StatsEnum.WATER_RESIST] },
                { 130, [StatsEnum.AIR_DOMAIN, StatsEnum.EARTH_DOMAIN, StatsEnum.FIRE_DOMAIN, StatsEnum.WATER_DOMAIN] },
                { 132, [StatsEnum.FIRE_RESIST] },
                { 161, [StatsEnum.RANGE] },
                { 168, [StatsEnum.CRIT_HIT] },
                { 172, [StatsEnum.INI] },
                { 174, [StatsEnum.TACKLE] },
                { 176, [StatsEnum.DODGE] },
                { 181, [StatsEnum.REAR_DOMAIN] },
                { 182, [StatsEnum.FIRE_DOMAIN] },
                { 192, [StatsEnum.WP] },
                { 876, [StatsEnum.BLOCK] },
                { 1056, [StatsEnum.CRIT_DOMAIN] },
                { 1059, [StatsEnum.MELE_DOMAIN] },
                { 1060, [StatsEnum.DISTANCE_DOMAIN] },
                { 1061, [StatsEnum.BERSERKER_DOMAIN] },
                { 1062, [StatsEnum.CRIT_RESISTANCE] },
                { 1063, [StatsEnum.REAR_RESISTANCE] }
            };


            foreach (var effect in item.Definition.EquipEffectsParsed)
            {
                if (item.Definition.Item.Id == 31904)
                {

                }
                if (dictionaryAdd.TryGetValue(effect.ActionId, out var effectsAdd))
                {
                    var valueAnatomy = 0.0;
                    foreach (var stat in effectsAdd)
                    {
                        var value = effect.Value;
                        var elemental = new StatsEnum[] { StatsEnum.AIR_DOMAIN, StatsEnum.EARTH_DOMAIN, StatsEnum.FIRE_DOMAIN, StatsEnum.WATER_DOMAIN }.Contains(stat);
                        if (applyElecon && elemental)
                            value *= 1.2;

                        if (isHupper && elemental)
                            value *= 1.2;

                        if (applyAlternance && elemental)
                            value *= 1.2;

                        if (anatomy & elemental)
                        {
                            var half = value / 2;
                            value = half;
                            valueAnatomy = half * 1.3;
                        }

                        stats.Add(stat, value);
                    }
                    if (anatomy) stats.Add(StatsEnum.REAR_DOMAIN, valueAnatomy);
                    continue;
                }

                if (dictionaryRemove.TryGetValue(effect.ActionId, out var effectsRem))
                {
                    foreach (var stat in effectsRem)
                    {
                        stats.Add(stat, -effect.Value);
                    }
                    continue;
                }

                var t = $"notHandled [{effect.ActionId}] {effect.ActionName}";
                var a = 2;
            }

            return stats;
        }
    }
}
