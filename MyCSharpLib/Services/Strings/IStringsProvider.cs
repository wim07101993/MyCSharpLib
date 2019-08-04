using System;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Interface that dedscribes the methods and properties of a strings service.
    /// </summary>
    public interface IStringsProvider<T> where T : IStrings
    {
        /// <summary>The Strings</summary>
        T Strings { get; }

        /// <summary>Is the service currently fetching strings.</summary>
        bool IsFetching { get; }
      
        /// <summary>Is the service saving strings.</summary>
        bool IsSaving { get; }

        /// <summary>Array of possible languages.</summary>
        string[] Languages { get; }

        /// <summary>Current selected language.</summary>
        string Language { get; set; }


        /// <summary>
        /// Updates the strings to the given language.
        /// </summary>
        /// <param name="language">The language to change to.</param>
        Task FetchStringsAsync(string language);

        /// <summary>
        /// Saves the strings to the specified path in the <see cref="Settings.Settings"/>.
        /// </summary>
        /// <param name="strings">Strings to save.</param>
        /// <param name="language">Language of the strings to save.</param>
        Task SaveStringsAsync(T strings, string language);

        /// <summary>
        /// Event that is fired when the settings are loaded.
        /// </summary>
        event EventHandler<T> FetchedStrings;
        /// <summary>
        /// Event that is fired when the settings are saved.
        /// </summary>
        event EventHandler<T> SavedStrings;
    }

    /// <summary>
    /// Interface that dedscribes the methods and properties of a strings service.
    /// </summary>
    public interface IStringsProvider
    {
        /// <summary>The Strings</summary>
        IStrings Strings { get; }

        /// <summary>Is the service currently fetching strings.</summary>
        bool IsFetching { get; }

        /// <summary>Is the service saving strings.</summary>
        bool IsSaving { get; }

        /// <summary>Array of possible languages.</summary>
        string[] Languages { get; }

        /// <summary>Current selected language.</summary>
        string Language { get; set; }


        /// <summary>
        /// Updates the strings to the given language.
        /// </summary>
        /// <param name="language">The language to change to.</param>
        Task FetchStringsAsync(string language);

        /// <summary>
        /// Saves the strings to the specified path in the <see cref="Settings.Settings"/>.
        /// </summary>
        /// <param name="strings">Strings to save.</param>
        /// <param name="language">Language of the strings to save.</param>
        Task SaveStringsAsync(IStrings strings, string language);

        /// <summary>
        /// Event that is fired when the settings are loaded.
        /// </summary>
        event EventHandler<IStrings> FetchedStrings;
        /// <summary>
        /// Event that is fired when the settings are saved.
        /// </summary>
        event EventHandler<IStrings> SavedStrings;
    }
}
