using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace Ism.Infrastructure.Validation
{
    public class Unique : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IServiceLocator serviceLocator = Services.ServiceLocator;

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
