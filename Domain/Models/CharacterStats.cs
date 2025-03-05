using Domain.Models.Stats;

namespace Domain.Models
{
    public class CharacterStats
    {
        public int HP { get; set; }
        public int AP { get; set; }
        public int MP { get; set; }
        public int WP { get; set; }
        //public DomainStats ElementalDomains { get; set; }
        //public ResistanceStats ElementalResistances { get; set; }
        //public SecondaryDomainStats SecondaryDomains { get; set; }
        //public SecondaryStats SecondaryStats { get; set; }
        //public CombatBaseStats CombatBaseStats { get; set; }
        public StatsCollection Stats { get; set; }

    }
}