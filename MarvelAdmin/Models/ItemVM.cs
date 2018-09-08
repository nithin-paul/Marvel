using Marvel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarvelAdmin.Models
{
    public class ItemVM
    {
        /// <summary>
        /// item id
        /// </summary>
        public int Id { get; set; }

        public int CategoryId { get; set; }
        
        public List<ImageModel> Images { get; set; }

        [Display(Name = "Offer Percentage")]
        public double? OfferPercent { get; set; } = 0;

        [Required(ErrorMessage = "Item Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Item Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Item Description is required")]
        public string Description { get; set; }

        [Display(Name = "Detail Description")]
        [Required(ErrorMessage = "Item Detail Description is required")]
        [MinLength(100, ErrorMessage = "Atleast 100 charectors should be there for an Item detail description")]
        public string DetailDescription { get; set; }

        public string ImageUrl { get; set; }
        
        public HttpPostedFileBase Image { get; set; }
    }
}