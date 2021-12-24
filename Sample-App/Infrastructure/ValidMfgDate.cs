using Sample_App.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Infrastructure
{
    public class ValidMfgDate : ValidationAttribute
    {
        public int Year { get; set; }

        //public string GetErrorMessage() => $"Mfg year should not be less than current year";

        public string ErrorMessageCustom { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var product = (ProductEditModel)validationContext.ObjectInstance;

            if (product.MfgDate.Year < DateTime.Now.Year)
            {
                //return new ValidationResult(GetErrorMessage());
                return new ValidationResult(ErrorMessageCustom);
            }
            return ValidationResult.Success;
            //return base.IsValid(value, validationContext);
        }
    }
}
