using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp9_MyFinances.ValidationRules;

public class NotEmptyValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        //bool notEmpty = (value as string).Length != 0;
        bool notEmpty = !string.IsNullOrEmpty(value as string);
        return new ValidationResult(notEmpty, "Can't be empty");
    }
}
