using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarvelAdmin.Models
{
    public class CategoryVM
    {
        /// <summary>
        /// Category id
        /// </summary>
        public int Id { get; set; }

        public int Oid { get; set; }

        /// <summary>
        /// Category Name
        /// </summary>
        [Required(ErrorMessage = "Category Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Description for category
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        /// <summary>
        /// Image for the category
        /// </summary>
        public HttpPostedFileBase Image { get; set; }
    }
}