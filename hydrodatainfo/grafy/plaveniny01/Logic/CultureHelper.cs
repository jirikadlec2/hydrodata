using System;
using System.Globalization;

namespace jk.plaveninycz
{
    public class CultureHelper
    {
        public static string CultureToShortString(CultureInfo culture)
        {
            string str;
            switch (culture.ToString())
            {
                case "cs-CZ":
                    str = "cz";
                    break;
                case "en-US":
                    str = "en";
                    break;
                default:
                    str = "en";
                    break;
            }
            return str;  
        }

        public static CultureInfo ShortStringToCulture(string abrev)
        {
            switch (abrev)
            {
                case "en":
                    return new CultureInfo("en-US");
                case "cz":
                    return new CultureInfo("cs-CZ");
                default:
                    return System.Threading.Thread.CurrentThread.CurrentCulture;
            }
        }

        public static string Default
        {
            get
            {
                CultureInfo c = System.Threading.Thread.CurrentThread.CurrentCulture;
                return CultureToShortString(c);
            }
        }
    }
}
