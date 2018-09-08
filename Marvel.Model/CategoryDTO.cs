using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Model
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public int Oid { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
    }
}
