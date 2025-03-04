using Domain.Enums;
using Domain.Models.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapperZenith.Extensions
{
    public static class SublimationExtension
    {
        public static StatsCollection GetStats(this string sublimationName, string build = "")
        {
            if (string.IsNullOrEmpty(sublimationName)) return new StatsCollection();
            var split = sublimationName.Split(' ');
            var lvl = GetLvL(split[split.Count()-1]);
            var name = build + " | " + string.Join(' ', split.Take(split.Count()-1).ToArray());
            var stats = new StatsCollection();
            var effectName = string.Join(' ', split.Take(split.Count() - 1).ToArray());
            switch (effectName.ToLower())
            {
                case "acribia":
                    stats.Add(Domain.Enums.StatsEnum.INFLICTED_DAMAGE, 6 * lvl, name);
                    break;
                case "quemadura":
                    stats.Add(Domain.Enums.StatsEnum.INFLICTED_DAMAGE, 4 * lvl, name);
                    break;
                case "ventilación":
                    stats.Add(Domain.Enums.StatsEnum.INFLICTED_DAMAGE, 4 * lvl, name);
                    break;
                case "telurismo":
                    stats.Add(Domain.Enums.StatsEnum.INFLICTED_DAMAGE, 4 * lvl, name);
                    break;
                case "congelación":
                    stats.Add(StatsEnum.INFLICTED_DAMAGE, 4 * lvl, name);
                    break;
                case "potencia bruta":
                    stats.Add(StatsEnum.INFLICTED_DAMAGE, 30, name);
                    break;
                case "focalización":
                    stats.Add(StatsEnum.INFLICTED_DAMAGE, 25, name);
                    break;
                case "valentía":
                    stats.Add(StatsEnum.INFLICTED_DAMAGE, 2 * lvl, name);
                    break;
                case "ruina":
                    stats.Add(StatsEnum.INDIRECT_DAMAGE, 5 * lvl, name);
                    break;
                case "ruina cíclica":
                    stats.Add(StatsEnum.INDIRECT_DAMAGE, 10 * lvl, name);
                    break;
                case "valor añadido":
                    stats.Add(StatsEnum.INDIRECT_DAMAGE, 40, name);
                    break;
                default:
                    if (string.IsNullOrEmpty(name)) break;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(effectName + " " + split[split.Count() - 1]);
                    Console.ResetColor();
                    break;

            }
            return stats;

            double GetLvL(string lvl) => lvl switch { "I" => 1, "II" => 2, "III" => 3, _ => throw new Exception("CANNOT identify lvl of sublimation") };
        }        
    }
}
