using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AISOS.Converters
{
    class PhoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string phoneNumber = value.ToString();

            switch (phoneNumber.Length)
            {
                case 6:
                    return Regex.Replace(phoneNumber, @"(\d{2})(\d{2})(\d{2})", "$1-$2-$3");
                case 7:
                    return Regex.Replace(phoneNumber, @"(\d{3})(\d{2})(\d{2})", "$1-$2-$3");
                case 10:
                    return Regex.Replace(phoneNumber, @"(\d{3})(\d{3})(\d{2})(\d{2})", "($1) $2-$3-$4");
                case 11:
                    return Regex.Replace(phoneNumber, @"(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})", "$1 ($2) $3-$4-$5");
                default:
                    return phoneNumber;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string phoneNumber = value.ToString();
            string r = "";
            for (int i = 0; i < phoneNumber.Length; i++)
            {
                char c = phoneNumber[i];
                if ('0' <= c && c <= '9') r += c;
            }
            long num = long.Parse(r);
            return num;
        }
    }
}
