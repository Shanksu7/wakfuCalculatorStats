using Domain.Enums;
using ZenithWebHandler.Models.Responses;

namespace ZenithWebHandler.Extensions
{
    public static class ItemZenithExtensions
    {
        public static void Elements(this ItemZenith item)
        {
            var domains = new ZenithInnerStatElementEnum[] { ZenithInnerStatElementEnum.FIRE_DOMAIN, ZenithInnerStatElementEnum.WATER_DOMAIN, ZenithInnerStatElementEnum.EARTH_DOMAIN };
            var resists = new ZenithInnerStatElementEnum[] { ZenithInnerStatElementEnum.FIRE_RESIST, ZenithInnerStatElementEnum.WATER_RESIST, ZenithInnerStatElementEnum.EARTH_RESIST };
            var randomResist = new List<int>() { };
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
                        else if (randomResist.Contains(effect.IdEffect.Value))
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
    }
}
