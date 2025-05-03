using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Validations
{
    public class PrimeiraLetraMaiuscula : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeiraLetra = value.ToString()[0].ToString();
            if(primeiraLetra != primeiraLetra.ToLower())
            {
                return new ValidationResult("A primeira letra d nome do produto deve ser maiúscula");
            }

            return ValidationResult.Success;
        }
    }
}