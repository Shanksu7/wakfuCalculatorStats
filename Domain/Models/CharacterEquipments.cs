using Domain.Models.Stats;
using Domain.Models.WAKFUAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WakfuItemsPlayground.Enums;

namespace Domain.Models
{
    public class CharacterEquipments
    {
        public void Add(params Dictionary<Item,double>[] args) 
        {
            foreach (var dict in args)
            {

                if (dict.Count == 0) continue;
                var item = dict.FirstOrDefault().Key;
                Items.Add(new (item.Definition.Item.BaseParameters.ItemTypeEnum, item));
                Stats += item.Definition.StatsCollection;
            }
        }

        public void Add(Item item, double args)
        {
            Items.Add(new(item.Definition.Item.BaseParameters.ItemTypeEnum, item));
            Stats += item.Definition.StatsCollection;
        }
        public List<Tuple<ItemTypesEnum, Item>> Items { get; set; } = new List<Tuple<ItemTypesEnum, Item>>();
        public StatsCollection Stats { get; internal set; } = new StatsCollection();

        public string Urls { 
            get 
            {
                var text = string.Empty;
                var urlArmor = "https://www.wakfu.com/es/mmorpg/enciclopedia/armaduras/{0}-{1}";
                var urlwep = "https://www.wakfu.com/es/mmorpg/enciclopedia/armas/{0}-{1}";
                foreach (var (_, item) in Items)
                {
                    
                    text += string.Format(urlArmor, item.Definition.Item.Id, item.Title.Es.Replace(" ", "-")) + "\n";
                }
                return text;
            } 
        }

    }
}
