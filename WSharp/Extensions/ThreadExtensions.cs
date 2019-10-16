using System.Threading;
using WSharp.Languages;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for a <see cref="Thread"/>.</summary>
    public static class ThreadExtensions
    {
        /// <summary>
        ///     Sets language of the application to the language of the culture of the current thread.
        /// </summary>
        /// <param name="thread">Thread to get the language from.</param>
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