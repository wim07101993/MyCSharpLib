using System;
using System.Globalization;
using System.Threading;

namespace WSharp.Languages
{
    public static class LanguageHelper
    {
        public static void SetLanguage(ELanguage language)
        {
            string lang = "";
            switch (language)
            {
                case ELanguage.Nl:
                    lang = "nl-NL";
                    break;
                case ELanguage.En:
                    lang = "en-GB";
                    break;
            }

            NumberFormatInfo numberInfo = CultureInfo.CreateSpecificCulture("nl-NL").NumberFormat;
            CultureInfo info = new CultureInfo(lang) { NumberFormat = numberInfo };
            //later, we will if-else the language here
            info.DateTimeFormat.DateSeparator = "/";
            info.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentUICulture = info;
            Thread.CurrentThread.CurrentCulture = info;
        }

        public static bool TrySetLanguage(ELanguage language)
        {
            try
            {
                SetLanguage(language);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
