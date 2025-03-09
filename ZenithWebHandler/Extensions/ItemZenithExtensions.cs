using Domain.Enums;
using Domain.Models.WAKFUAPI;
using WakfuItemsPlayground.Enums;
using ZenithWebHandler.Models.Responses;

namespace ZenithWebHandler.Extensions
{
    public static class ItemZenithExtensions
    {
        public static void Elements(this ItemZenith item)
        {
            var domains = new ZenithInnerStatElementEnum[] { ZenithInnerStatElementEnum.FIRE_DOMAIN, ZenithInnerStatElementEnum.WATER_DOMAIN, ZenithInnerStatElementEnum.EARTH_DOMAIN };
            var resists = new ZenithInnerStatElementEnum[] { ZenithInnerStatElementEnum.FIRE_RESIST, ZenithInnerStatElementEnum.WATER_RESIST, ZenithInnerStatElementEnum.EARTH_RESIST };
            var randomResist = new List<int>() { 1069 };
            var randomDomain = new List<int>() { 1068 };
            foreach (var effect in item.Effects)
            {
                foreach (var value in effect.Values)
                {
                    if (value.RandomNumber.HasValue && value.RandomNumber.Value > 0)
                    {
                        if (randomDomain.Contains(value.IdStats.Value))
                        {
                            value.Elements = GetElement(domains, value.RandomNumber.Value);
                        }
                        else if (randomResist.Contains(value.IdStats.Value))
                        {
                            value.Elements = GetElement(resists, value.RandomNumber.Value);
                        }
                        else
                            throw new Exception($"Not handled random value {effect.NameEffect} for id : " + effect.IdEffect);
                    }
                }
            }
        }

        static List<Elements> GetElement(ZenithInnerStatElementEnum[] stats, int random)
        {
            var result = new List<Elements>();
            for (int i = 0; i < random; i++)
                result.Add(new Elements() { IdElement = i + 1, IdInnerStats = (int)stats[i], ImageElement = GetPng(stats[i]) });
            return result;
        }

        static string GetPng(ZenithInnerStatElementEnum stat)
        {
            switch (stat)
            {
                case ZenithInnerStatElementEnum.WATER_DOMAIN:
                case ZenithInnerStatElementEnum.WATER_RESIST:
                    return "water.png";

                case ZenithInnerStatElementEnum.FIRE_DOMAIN:
                case ZenithInnerStatElementEnum.FIRE_RESIST:
                    return "fire.png";

                case ZenithInnerStatElementEnum.EARTH_DOMAIN:
                case ZenithInnerStatElementEnum.EARTH_RESIST:
                    return "earth.png";
                default: throw new Exception("Not implemented");

            }
        }

        public static ItemZenith AddMetaData(this ItemZenith _item, Item item, ItemTypesEnum[] firstHand = null, ItemTypesEnum[] secondhand= null, bool isFirstRing = false)
        {
            var itemType = item.Definition.Item.BaseParameters.ItemType.ItemTypeEnum;
            ZenithSideEquipEnum metaSide;
            switch (itemType)
            {
                case ItemTypesEnum.Casco: metaSide = ZenithSideEquipEnum.HELM; break;
                case ItemTypesEnum.Amuleto: metaSide = ZenithSideEquipEnum.NECKLACE; break;
                case ItemTypesEnum.Coraza: metaSide = ZenithSideEquipEnum.BREATSPLACE; break;
                case ItemTypesEnum.Anillo:
                    metaSide = isFirstRing ? ZenithSideEquipEnum.LEFT_RING : ZenithSideEquipEnum.RIGHT_RING;
                    isFirstRing = false;
                    break;
                case ItemTypesEnum.Botas: metaSide = ZenithSideEquipEnum.BOOTS; break;
                case ItemTypesEnum.Capa: metaSide = ZenithSideEquipEnum.CAPE; break;
                case ItemTypesEnum.Hombreras: metaSide = ZenithSideEquipEnum.SHOULDERS; break;
                case ItemTypesEnum.Cinturon: metaSide = ZenithSideEquipEnum.BELT; break;
                case ItemTypesEnum.Emblema: metaSide = ZenithSideEquipEnum.EMBLEM; break;
                case ItemTypesEnum.Mascota: metaSide = ZenithSideEquipEnum.PET; break;
                case ItemTypesEnum.Mount: metaSide = ZenithSideEquipEnum.MOUNT; break;
                default:
                    {
                        var oneHandType = firstHand;
                        var twoHandType = firstHand;
                        var secondHandType = secondhand;

                        if (oneHandType.Contains(itemType) || twoHandType.Contains(itemType))
                        {
                            metaSide = ZenithSideEquipEnum.FIRST_HAND;
                            break;
                        }

                        if (secondHandType.Contains(itemType))
                        {
                            metaSide = ZenithSideEquipEnum.SECOND_HAND;
                            break;
                        }

                        throw new Exception("Not handled: " + itemType);
                    }
            }
            _item.Metadata = new Metadata() { Side = (int)metaSide };
            return _item;
        }
    }
}
