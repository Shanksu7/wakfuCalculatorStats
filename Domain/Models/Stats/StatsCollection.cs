using Domain.Enums;
using Domain.Models.Stats.Combat;
using System.Numerics;

namespace Domain.Models.Stats
{
    public class StatsCollection
    {
        public StatsCollection()
        {
            Stats = new Dictionary<StatsEnum, double>();
        }

        Dictionary<StatsEnum, double> Stats { get; set; }

        public double this[StatsEnum stat] 
        {
            get
            {
                if (Stats.TryGetValue(stat, out var value)) return value;
                return 0.0;
            }
            set
            {
                if (Stats.TryGetValue(stat, out _))
                {
                    Stats[stat] = value;
                }
                else
                    Stats.TryAdd(stat, value);
            }
        }

        public double Add(StatsEnum stat, double value, string description = null)
        {
            if(value != 0 && description is not null)
            {
                Console.WriteLine($"Added {description}: {value} : [{stat.ToString()}]");
            }
            return this[stat] += value;
        }

        public bool Contains(StatsEnum stat, out double value) => Stats.TryGetValue(stat, out value);

        public static StatsCollection operator +(StatsCollection stats, StatsCollection stats2)
        {
            var values = Enum.GetValues<StatsEnum>();
            var result = new StatsCollection();
            foreach (var stat in values)
            {
                if (stats[stat] == 0 && stats2[stat] == 0) 
                    continue;
                result[stat] = stats[stat] + stats2[stat];
            }

            return result;
        }

        public double CalculateDamage(CalculateDamage c, StatsCollection addedStats = null)
        {
            var domainBase = 0.0;
            var resistanceBase = 0.0;
            var resistancePercentage = 0.0;

            if (addedStats != null) 
                foreach(var (stat, value) in addedStats.Stats) Add(stat, value);

            switch (c.DomainType)
            {
                case DomainType.FIRE:
                    domainBase = this[StatsEnum.FIRE_DOMAIN];
                    resistanceBase = c.EnemyStats[StatsEnum.FIRE_RESIST];
                    resistancePercentage = c.FixedResistancePercent ?? GetResistPercentage(resistanceBase);
                    break;
                case DomainType.WATER:
                    domainBase = this[StatsEnum.WATER_DOMAIN];
                    resistanceBase = c.EnemyStats[StatsEnum.WATER_RESIST];
                    resistancePercentage = c.FixedResistancePercent ?? GetResistPercentage(resistanceBase);
                    break;
                case DomainType.AIR:
                    domainBase = this[StatsEnum.AIR_DOMAIN];
                    resistanceBase = c.EnemyStats[StatsEnum.AIR_RESIST];
                    resistancePercentage = c.FixedResistancePercent ?? GetResistPercentage(resistanceBase);
                    break;
                case DomainType.EARTH:
                    domainBase = this[StatsEnum.EARTH_DOMAIN];
                    resistanceBase = c.EnemyStats[StatsEnum.EARTH_RESIST];
                    resistancePercentage = c.FixedResistancePercent ?? GetResistPercentage(resistanceBase);
                    break;
            }
            var secondary = GetSubDomain(c);
            var applicableDomain = domainBase + secondary;
            var a1 = c.BaseDamage * (applicableDomain / 100.0);
            var damageInflictedAugment = GetInflictedDamage(c);
            var a2 = (a1 + c.BaseDamage) * damageInflictedAugment;
            var result = a2 * ((100.0 - resistancePercentage) / 100.0);


            if (addedStats != null)
                foreach (var (stat, value) in addedStats.Stats) Add(stat, -value);

            return result;
        }

        double GetResistPercentage(double score)
        {
            var result = (1.0 - Math.Pow(0.8, score / 100.0)) * 100.0;

            return result > 90.0 ? 90.0 : result;
        }

        double GetSubDomain(CalculateDamage c)
        {
            var domainResult = 0.0;
            if (c.IsHealing)
            {
                domainResult += this[StatsEnum.HEAL_DOMAIN];
                domainResult += GetRangeBonusDomain(c.RangeDamageType);
                if (c.IsCrit) domainResult += this[StatsEnum.CRIT_DOMAIN];
                return domainResult;
            }   

            if (c.IsCrit) domainResult += this[StatsEnum.CRIT_DOMAIN];

            if (c.Side == SideDamage.REAR)
                if(!c.IsIndirect)
                    domainResult += this[StatsEnum.REAR_DOMAIN];

            if (c.IsBerserker) domainResult += this[StatsEnum.BERSERKER_DOMAIN];
            domainResult += GetRangeBonusDomain(c.RangeDamageType);

            return domainResult;

            double GetRangeBonusDomain(RangeDamageEnum range)
            {
                return range switch
                {
                    RangeDamageEnum.DIST => this[StatsEnum.DISTANCE_DOMAIN],
                    RangeDamageEnum.MELE => this[StatsEnum.MELE_DOMAIN],
                    RangeDamageEnum.HIGHEST => Math.Max(this[StatsEnum.MELE_DOMAIN], this[StatsEnum.DISTANCE_DOMAIN])

                };
            }
        }

        double GetInflictedDamage(CalculateDamage c)
        {
            var result = this[StatsEnum.INFLICTED_DAMAGE] + (c.IsIndirect ? this[StatsEnum.INDIRECT_DAMAGE] : 0.0);

            if (c.RangeDamageType == RangeDamageEnum.DIST) result += this[StatsEnum.INFLICTED_DAMAGE_DIST];
            if (c.RangeDamageType == RangeDamageEnum.MELE) result += this[StatsEnum.INFLICTED_DAMAGE_MELE];
            if (c.RangeDamageType == RangeDamageEnum.HIGHEST) result += Math.Max(this[StatsEnum.INFLICTED_DAMAGE_DIST], this[StatsEnum.INFLICTED_DAMAGE_MELE]);
            
            return (result / 100.0) + 1.0;
        }
    }
}