using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI
{
    public class RecipeCategoriesDefinition
    {
        public int Id { get; set; }
        public bool IsArchive { get; set; }
        public bool IsNoCraft { get; set; }
        public bool IsHidden { get; set; }
        public int XpFactor { get; set; }
        public bool IsInnate { get; set; }
    }

    public class RecipeCategories
    {
        public RecipeCategoriesDefinition Definition { get; set; }
        public Lang Title { get; set; }
    }

}
