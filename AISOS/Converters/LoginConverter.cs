using AISOS.Windows;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Forms;

namespace AISOS.Converters
{
    class LoginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value.ToString();
            Regex login_regex = new Regex("^[a-zA-Z0-9]{0,}$");

            if (login_regex.Match(text).Success) return text;
            else
            {
                OkBox.Show("Можно использовать только латинские символы (a-z A-Z) и цифры (0-9)");
                string r = "";
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (('a' <= c && c <= 'z') || ('0' <= c && c <= '9')) r += c;
                }
                return r;
            }
        }
    }
}
