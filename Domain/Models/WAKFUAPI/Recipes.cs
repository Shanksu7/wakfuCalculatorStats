using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI
{
    public class Recipes
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int Level { get; set; }
        public int XpRatio { get; set; }
        public bool IsUpgrade { get; set; }
        public int UpgradeItemId { get; set; }
    }
}
