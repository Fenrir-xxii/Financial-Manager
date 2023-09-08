using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp9_MyFinances.Converters;

public class LocalizeCurrencyMultiConverter : System.Windows.Data.IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        decimal originalAmount;
        if (!decimal.TryParse(values[0].ToString(), out originalAmount))
            return values[0];
        try
        {
            int v = (int)values[1];
            if (v == 980)
            {
                return string.Format(System.Globalization.CultureInfo.GetCultureInfo("uk-UA"), "{0:c}", originalAmount);
            }
            else if (v == 840)
            {
                var frenchCulture = new CultureInfo("fr");
                frenchCulture.NumberFormat.CurrencySymbol = "$";
                return string.Format(frenchCulture, "{0:c}", originalAmount);

                //return string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:c}", originalAmount);
            }
            else if (v == 978)
            {
                
                return string.Format(System.Globalization.CultureInfo.GetCultureInfo("fr-FR"), "{0:c}", originalAmount);
            }
            else if (v == 826)
            {
                var frenchCulture = new CultureInfo("fr");
                frenchCulture.NumberFormat.CurrencySymbol = "£";
                return string.Format(frenchCulture, "{0:c}", originalAmount);

                //return string.Format(System.Globalization.CultureInfo.GetCultureInfo("uk"), "{0:c}", originalAmount);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de");

                var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
                nfi.CurrencySymbol = "|*|";
                return string.Format(System.Globalization.CultureInfo.GetCultureInfo("de"), "{0:c}", originalAmount);
            }
        }catch (Exception ex)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:N2}", originalAmount); ;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
