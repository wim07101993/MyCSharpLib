using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;

namespace MyCSharpLib.Wpf.Controls.ObjectBrowser
{
    public class ObjectBrowserPropertyValidationRule : ValidationRule
    {
        public ValidationAttributesWrapper ValidationAttributes { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (ValidationAttributes == null || ValidationAttributes.Value == null)
                return ValidationResult.ValidResult;

            foreach (var validationAttribute in ValidationAttributes.Value)
            {
                if (validationAttribute.IsValid(value))
                    continue;

                return new ValidationResult(false, validationAttribute.ErrorMessage);
            }

            return ValidationResult.ValidResult;
        }
    }
}
