using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI
{
    public class StatesDefinition
    {
        public int Id { get; set; }
    }

    public class States
    {
        public Definition Definition { get; set; }
        public Lang Title { get; set; }
    }

}
