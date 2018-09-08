using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Model
{
    public class ItemDTO
    {
        /// <summary>
        /// Category id
        /// </summary>
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public List<ImageModel> Images { get; set; }
        
        public double Price { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public string DetailDescription { get; set; }

        public string ImageUrl { get; set; }

        public double OfferPercent { get; set; }
    }
}
