using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Stats.Domains
{
    public class DomainBase
    {
        public DomainBase(double water, double earth, double air, double fire)
        {
            Water = water;
            Earth = earth;
            Air = air;
            Fire = fire;
        }

        public double Water { get; set; }
        public double Earth { get; set; }
        public double Air { get; set; }
        public double Fire { get; set; }
    }
}
