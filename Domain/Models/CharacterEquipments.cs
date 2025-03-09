using Domain.Enums;
using Domain.Models.Stats;
using Domain.Models.WAKFUAPI;
using WakfuItemsPlayground.Enums;

namespace Domain.Models
{
    public class CharacterEquipments
    {
        public CharacterEquipments(bool isXelor = false)
        {
            Stats[StatsEnum.AP] += 6;
            Stats[StatsEnum.MP] += 3;
            Stats[StatsEnum.WP] += isXelor ? 12 : 6;
        }

        public void Add(params Dictionary<Item, double>[] args)
        {
            foreach (var dict in args)
            {

                if (dict.Count == 0) continue;
                var item = dict.FirstOrDefault().Key;
                Items.Add(new(item.Definition.Item.BaseParameters.ItemType.ItemTypeEnum, item));
                Stats += item.Definition.StatsCollection;
            }
        }

        List<string> OcupiedPositions = new List<string>();
        List<string> DisabledPositions = new List<string>();


        public void AddRelicEpic(Dictionary<Item, (Item, double)> compared)
        {
            Dictionary<Item, double> result = new();
            foreach (var x in compared)
                result.Add(x.Key, 0);

            var added = false;
            foreach (var (key, _) in compared)
            {
                added = Add(key);
                if (added) break;
            }
            if (!added) throw new Exception("a?");
        }

        public void AddSingle(Dictionary<Item, double> dict)
        {
            if (dict.Any())
            {
                foreach (var (item, _) in dict)
                {
                    var added = Add(dict.FirstOrDefault().Key);
                    if (!added) continue;
                    else break;
                }
            }
        }

        public bool Add(Item item)
        {
            var itemType = item.Definition.Item.BaseParameters.ItemType;
            var itemRarity = item.Definition.Item.BaseParameters.Rarity;
            KeyValuePair<ItemTypesEnum, Item> info = new(itemType.ItemTypeEnum, item);

            var positions = new List<(string, bool)>();

            foreach (var position in itemType.Definition.EquipmentPositions)
            {
                positions.Add((position, OcupiedPositions.Contains(position) || DisabledPositions.Contains(position)));
            }

            if (positions.TrueForAll(x => x.Item2 == true))
            {
                return false;
            }

            if ((ItemRarity)itemRarity == ItemRarity.EPIC)
                if (Items.Count(x => (ItemRarity)x.Value.Definition.Item.BaseParameters.Rarity == ItemRarity.EPIC) > 0) return false;

            if ((ItemRarity)itemRarity == ItemRarity.RELIC)
                if (Items.Count(x => (ItemRarity)x.Value.Definition.Item.BaseParameters.Rarity == ItemRarity.RELIC) > 0) return false;

            var added = false;
            foreach (var position in itemType.Definition.EquipmentPositions)
            {
                if (!OcupiedPositions.Contains(position))
                {
                    OcupiedPositions.Add(position);
                    added = true;
                    break;
                }
            }

            if (!added) throw new Exception($"Something happening on adding {item}");

            foreach (var position in itemType.Definition.EquipmentDisabledPositions)
            {
                if (!DisabledPositions.Contains(position))
                {
                    DisabledPositions.Add(position);
                    break;
                }
            }

            if (itemType.ItemTypeEnum == ItemTypesEnum.Anillo)
            {
                if (Items.Count(x => x.Key == ItemTypesEnum.Anillo) < 2)
                {
                    Items.Add(info);
                    Stats += item.Definition.StatsCollection;
                }
            }
            else
            {
                if (Items.Count(x => x.Key == itemType.ItemTypeEnum) == 0)
                {
                    Items.Add(info);
                    Stats += item.Definition.StatsCollection;

                }
                else
                {
                    Console.WriteLine("Already present: " + itemType);
                }
            }

            Console.WriteLine($"Using: {item.ToString()}");
            return true;
        }

        public List<KeyValuePair<ItemTypesEnum, Item>> Items { get; set; } = new List<KeyValuePair<ItemTypesEnum, Item>>();
        public StatsCollection Stats { get; set; } = new StatsCollection();

        public string Urls
        {
            get
            {
                var text = string.Empty;
                var urlArmor = "https://www.wakfu.com/es/mmorpg/enciclopedia/armaduras/{0}-{1}";
                var urlwep = "https://www.wakfu.com/es/mmorpg/enciclopedia/armas/{0}-{1}";
                foreach (var (_, item) in Items)
                {

                    text += string.Format(urlArmor, item.Definition.Item.Id, item.Title.Es.Replace(" ", "-").Replace(".", " ")) + "\n";
                }
                return text;
            }
        }

    }
}
