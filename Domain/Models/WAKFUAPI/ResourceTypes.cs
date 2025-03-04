using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI
{
   public class ResourceTypesDefinition
    {
        public int Id { get; set; }
        public bool AffectWakfu { get; set; }
    }

    public class ResourceTypes
    {
        public Definition Definition { get; set; }
        public Lang Title { get; set; }
    }

}
