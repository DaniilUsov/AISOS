using AISOS.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISOS
{
    public static class Constants
    {
        public static IFormatProvider DATE_FORMAT = new CultureInfo("ru-RU");
        public static User CurrenUser { get; set; }
    }
}
