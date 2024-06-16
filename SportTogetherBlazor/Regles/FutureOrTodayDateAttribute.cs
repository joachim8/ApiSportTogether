using System;
using System.ComponentModel.DataAnnotations;
namespace SportTogetherBlazor.Regles
{


    public class FutureOrTodayDateAttribute : ValidationAttribute
    {
        public FutureOrTodayDateAttribute() : base("La date doit être aujourd'hui ou dans le futur.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue.Date >= DateTime.Now.Date)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return new ValidationResult("Invalid date format.");
        }
    }

}
