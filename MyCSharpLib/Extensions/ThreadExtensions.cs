using MyCSharpLib.Languages;
using System.Threading;

namespace MyCSharpLib.Extensions
{
    public static class ThreadExtensions
    {
        public static void SetLanguage(this Thread thread)
        {
            switch (thread.CurrentCulture.ToString())
            {
                case "nl-NL":
                    Language.Culture = new System.Globalization.CultureInfo("nl-NL");
                    break;
                case "en-GB":
                    Language.Culture = new System.Globalization.CultureInfo("en-GB");
                    break;
                default://default english because there can be so many different system language, we rather fallback on english in this case.
                    Language.Culture = new System.Globalization.CultureInfo("en-GB");
                    break;
            }
        }
    }
}
