using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI
{
    public class HarvestLoots
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int RequiredProspection { get; set; }
        public double DropRate { get; set; }
        public int ListId { get; set; }
        public int QuantityPerItem { get; set; }
        public int QuantityMin { get; set; }
        public int QuantityMax { get; set; }
        public int MaxRoll { get; set; }
        public bool ItemIsLootList { get; set; }
    }

}
