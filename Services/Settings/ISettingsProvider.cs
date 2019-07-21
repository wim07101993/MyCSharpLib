using System;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Provider to get <see cref="Settings"/> from the settings of the application.
    /// </summary>
    /// <typeparam name="T">Type of settings to handle.</typeparam>
    public interface ISettingsProvider<T> where T : class, ISettings
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
        /// <param name="settings">Settings to save. If this parameter is null, the property value is used.</param>
        Task SaveSettingsAsync(T settings = null);

        /// <summary>
        /// Event that is fired when the settings are loaded.
        /// </summary>
        event EventHandler<T> FetchedSettings;
        /// <summary>
        /// Event that is fired when the settings are saved.
        /// </summary>
        event EventHandler<T> SavedSettings;
    }

    /// <summary>
    /// Provider to get <see cref="Settings"/> from the settings of the application.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>The settings</summary>
        ISettings Settings { get; }

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
        /// <param name="settings">Settings to save. If this parameter is null, the property value is used.</param>
        Task SaveSettingsAsync(ISettings settings = null);

        /// <summary>
        /// Event that is fired when the settings are loaded.
        /// </summary>
        event EventHandler<ISettings> FetchedSettings;
        /// <summary>
        /// Event that is fired when the settings are saved.
        /// </summary>
        event EventHandler<ISettings> SavedSettings;
    }
}
