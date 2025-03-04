using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Stats.Secondary
{
    public class SecondaryDomainStats
    {
        public double Critical { get; set; }
        public double Rear { get; set; }
        public double Mele { get; set; }
        public double Distance { get; set; }
        public double Heal { get; set; }
        public double Berserker { get; set; }
        public double GetSubDomain(SideDamage sideDamage, bool isCrit,bool isDistance, bool isBerserker, bool isHealing)
        {
            var domainResult = 0.0;
            if(isHealing)
            {
                domainResult += Heal;
                domainResult += isDistance ? Distance : Mele;
                if (isCrit) domainResult += Critical;
                return domainResult;
            }

            if (isCrit) domainResult += Critical;
            if (sideDamage == SideDamage.REAR) domainResult += Rear;
            if (isBerserker) domainResult += Berserker;
            domainResult += isDistance ? Distance : Mele;

            return domainResult;
        }
    }
}