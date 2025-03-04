using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI.Parsed
{
    public class EquipEffectParsed
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public double Value { get; set; }
        public override string ToString()
        {
            return $"[{ActionId}] {ActionName} : {Value}";
        }
    }
}
