using Domain.Enums;

namespace Domain.Models.Stats.Combat
{
    public class CalculateDamage
    {
        public CalculateDamage(double baseDamage, DomainType type, StatsCollection resistance, SideDamage sideDamage, bool isCrit, RangeDamageEnum rangeDamageEnum, bool isBerserker, bool isHealing, bool isIndirect, double? resist = null)
        {
            this.BaseDamage = baseDamage;
            this.DomainType = type;
            this.EnemyStats = resistance;
            Side = sideDamage;
            IsCrit = isCrit;
            RangeDamageType = rangeDamageEnum;
            IsBerserker = isBerserker;
            IsHealing = isHealing;
            IsIndirect = isIndirect;
            FixedResistancePercent = resist;
        }
        public double BaseDamage { get; set; }
        public DomainType DomainType { get; set; }
        public StatsCollection EnemyStats { get; set; }
        public SideDamage Side { get; set; }
        public bool IsCrit { get; set; }
        public RangeDamageEnum RangeDamageType { get; set; }
        public bool IsBerserker { get; set; }
        public bool IsHealing { get; set; }
        public bool IsIndirect { get; set; }
        public double? FixedResistancePercent { get; set; } = null;
    }
}
