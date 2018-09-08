using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Model
{
    public class ImageModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int ItemId { get; set; }
        
        public bool IsPrimary { get; set; }
    }
}
