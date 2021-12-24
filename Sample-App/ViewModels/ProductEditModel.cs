using Microsoft.AspNetCore.Mvc;
using Sample_App.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.ViewModels
{
    public class ProductEditModel
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Desc is required")]
        [StringLength(20, ErrorMessage = "{0} must be between {2} {1}", MinimumLength = 5)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 1000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Remote(action: "VerifyCategory", controller: "Home")]
        public string Category { get; set; }

        [Display(Name = "Mfg Date")]
        [ValidMfgDate(ErrorMessageCustom ="Error...")]
        public DateTime MfgDate { get; set; }
    }
}
