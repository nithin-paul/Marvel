using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Marvel.Models
{
    public class SliderVM
    {
        /// <summary>
        /// slider id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Description for slider
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        /// <summary>
        /// Description for slider
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        /// <summary>
        /// Image for the slider
        /// </summary>
        public HttpPostedFileBase Image { get; set; }
    }
}