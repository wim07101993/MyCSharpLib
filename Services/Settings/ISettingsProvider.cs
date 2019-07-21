using System;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    public interface ISettingsProvider<T> where T : Settings
    {
        /// <summary>The settings</summary>
        T Settings { get; }

        /// <summary>Is the service currently fetching settings.</summary>
        bool IsFetching { get; }

        /// <summary>Is the service saving settings.</summary>
        bool IsSaving { get; }


        /// <summary>
        /// Fetches the settings. All the properties of the <see cref="Settings"/> property are updated instead
        /// of returning a new instance. This way there is only one instance that is constantly updated.
        /// </summary>
        Task FetchSettingsAsync();

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settings">Settings to save.</param>
        Task SaveSettingsAsync(T settings);

        /// <summary>
        /// Event that is fired when the settings are loaded.
        /// </summary>
        event EventHandler<T> FetchedSettings;
        /// <summary>
        /// Event that is fired when the settings are saved.
        /// </summary>
        event EventHandler<T> SavedSettings;
    }

    public interface ISettingsProvider
    {
        /// <summary>The settings</summary>
        Settings Settings { get; }

        /// <summary>Is the service currently fetching settings.</summary>
        bool IsFetching { get; }

        /// <summary>Is the service saving settings.</summary>
        bool IsSaving { get; }


        /// <summary>
        /// Fetches the settings. All the properties of the <see cref="Settings"/> property are updated instead
        /// of returning a new instance. This way there is only one instance that is constantly updated.
        /// </summary>
        Task FetchSettingsAsync();

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settings">Settings to save.</param>
        Task SaveSettingsAsync(Settings settings);

        /// <summary>
        /// Event that is fired when the settings are loaded.
        /// </summary>
        event EventHandler<Settings> FetchedSettings;
        /// <summary>
        /// Event that is fired when the settings are saved.
        /// </summary>
        event EventHandler<Settings> SavedSettings;
    }
}
