using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Abstraction layer for the <see cref="ISettingsProvider"/> and <see cref="ISettingsProvider{T}"/>.
    /// Provider to get <see cref="Settings"/> from the settings of the application.
    /// </summary>
    /// <typeparam name="T">Type of settings to handle.</typeparam>
    public abstract class ASettingsProvider<T> : ISettingsProvider<T>, ISettingsProvider where T : class, ISettings, new()
    {
        #region FIELDS

        /// <summary>
        /// A dictionary that is used to store all the subscribtions to the <see cref="ISettingsProvider.FetchedSettings"/> event.
        /// </summary>
        private readonly Dictionary<EventHandler<ISettings>, EventHandler<T>> _convertedFetchedSettingsEventHandlers = new Dictionary<EventHandler<ISettings>, EventHandler<T>>();

        /// <summary>
        /// A dictionary that is used to store all the subscribtions to the <see cref="ISettingsProvider.SavedSettings"/> event.
        /// </summary>
        private readonly Dictionary<EventHandler<ISettings>, EventHandler<T>> _convertedSavedSettingsEventHandlers = new Dictionary<EventHandler<ISettings>, EventHandler<T>>();

        #endregion FIELDS


        #region PROPERTIES

        /// <summary>
        /// The settings. It is initialised with the default value.
        /// Once the <see cref="FetchSettingsAsync"/> method is invoked, it will contain the actual settings.
        /// </summary>
        public virtual T Settings { get; } = new T();

        /// <summary>
        /// The settings cast to <see cref="Services.Settings"/>. It is initialised with the default value.
        /// Once the <see cref="FetchSettingsAsync"/> method is invoked, it will contain the actual settings.
        /// </summary>
        ISettings ISettingsProvider.Settings => Settings;

        /// <summary>Is the service currently fetching settings.</summary>
        public bool IsFetching { get; private set; }
     
        /// <summary>Is the service saving settings.</summary>
        public bool IsSaving { get; private set; }

        #endregion PROPERTIES


        #region METHODS

        /// <summary>
        /// Internal method to perform the actual fetching of the settings.
        /// It is called by the <see cref="FetchSettingsAsync"/> method after setting the <see cref="IsFetching"/> property on true.
        /// Once the settings are returned, they will be copied to the <see cref="Settings"/> property and the <see cref="IsFetching"/> flag
        /// is set to false.
        /// After that the <see cref="FetchedSettings"/> events are fired and the settings are also automatically saved again.
        /// </summary>
        /// <returns>The setting for the application.</returns>
        protected abstract Task<T> InternalFetchSettingsAsync();

        /// <summary>
        /// Fetches the settings. All the properties of the <see cref="Settings"/> property are updated instead
        /// of returning a new instance. This way there is only one instance that is constantly updated.
        /// </summary>
        public async Task FetchSettingsAsync()
        {
            IsFetching = true;

            T settings = await InternalFetchSettingsAsync();
            CopySettingsFrom(settings);

            IsFetching = false;

            RaiseFetchedSettingsEvent();

            await SaveSettingsAsync(Settings);
        }

        /// <summary>
        /// Internal method to perform the actual saving of the settings.
        /// It is called by the <see cref="SaveSettingsAsync(T)"/> methods after setting the <see cref="IsSaving"/> proeprty on true
        /// and copying the settings to the <see cref="Settings"/> property.
        /// Once the saving is done the <see cref="IsSaving"/> flag is automatically set to false.
        /// After that the <see cref="SavedSettings"/> events are fired.
        /// </summary>
        /// <param name="settings">The settings to save.</param>
        /// <returns></returns>
        protected abstract Task InternalSaveSettingsAsync(T settings);

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settings">Settings to save. If this parameter is null, the property value is used.</param>
        public async Task SaveSettingsAsync(T settings = null)
        {
            IsSaving = true;

            if (settings != null)
                CopySettingsFrom(settings);

            await InternalSaveSettingsAsync(Settings);

            IsSaving = false;

            RaiseSavedSettingsEvent();
        }

        /// <summary>
        /// Saves the settings. (Just calls the <see cref="SaveSettingsAsync(T)"/> method.
        /// </summary>
        /// <param name="settings">Settings to save. If this parameter is null, the property value is used.</param>
        public async Task SaveSettingsAsync(ISettings settings = null)
        {
            if (settings == null)
            {
                await SaveSettingsAsync(null as T);
                return;
            }

            if (!(settings is T s))
                throw new ArgumentException("Invalid settings type.");

            await SaveSettingsAsync(s);
        }

        /// <summary>Raises the <see cref="FetchedSettings"/> events.</summary>
        protected virtual void RaiseFetchedSettingsEvent() => FetchedSettings?.Invoke(this, Settings);

        /// <summary>Raises the <see cref="SavedSettings"/> events.</summary>
        protected virtual void RaiseSavedSettingsEvent() => SavedSettings?.Invoke(this, Settings);

        /// <summary>
        /// Copies the settings from the parameter to the property <see cref="Settings"/>.
        /// </summary>
        /// <param name="settings">The settings to copy.</param>
        protected void CopySettingsFrom(T settings)
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(x => x.CanWrite && x.CanRead);

            foreach (var property in properties)
                property.SetValue(Settings, property.GetValue(settings));
        }

        #endregion METHODS


        #region EVENTS

        /// <summary>
        /// Event that is fired when the settings are loaded.
        /// </summary>
        public event EventHandler<T> FetchedSettings;

        /// <summary>
        /// Event that is fired when the settings are loaded. It converts the EventHandler to one for the <see cref="ISettingsProvider{T}.FetchedSettings"/>>
        /// and uses that to subscribe to the event.
        /// When removing the subsciption is retrieved from the <see cref="_convertedFetchedSettingsEventHandlers"/> and removed.
        /// </summary>
        event EventHandler<ISettings> ISettingsProvider.FetchedSettings
        {
            add
            {
                var convertedEventHandler = new EventHandler<T>((sender, e) => value(sender, e));
                if (!_convertedFetchedSettingsEventHandlers.ContainsKey(value))
                {
                    _convertedFetchedSettingsEventHandlers.Add(value, convertedEventHandler);
                    FetchedSettings += convertedEventHandler;
                }
            }

            remove
            {
                if (!_convertedFetchedSettingsEventHandlers.ContainsKey(value))
                    return;

                var convertedEventHandler = _convertedFetchedSettingsEventHandlers[value];
                FetchedSettings -= convertedEventHandler;
                _convertedFetchedSettingsEventHandlers.Remove(value);
            }
        }

        /// <summary>
        /// Event that is fired when the settings are saved.
        /// </summary>
        public event EventHandler<T> SavedSettings;

        /// <summary>
        /// Event that is fired when the settings are saved. It converts the EventHandler to one for the <see cref="ISettingsProvider{T}.SavedSettings"/>>
        /// and uses that to subscribe to the event.
        /// When removing the subsciption is retrieved from the <see cref="_convertedSavedSettingsEventHandlers"/> and removed.
        /// </summary>
        event EventHandler<ISettings> ISettingsProvider.SavedSettings
        {
            add
            {
                var convertedEventHandler = new EventHandler<T>((sender, e) => value(sender, e));
                if (!_convertedSavedSettingsEventHandlers.ContainsKey(value))
                {
                    _convertedSavedSettingsEventHandlers.Add(value, convertedEventHandler);
                    SavedSettings += convertedEventHandler;
                }
            }

            remove
            {
                if (!_convertedSavedSettingsEventHandlers.ContainsKey(value))
                    return;

                var convertedEventHandler = _convertedSavedSettingsEventHandlers[value];
                SavedSettings -= convertedEventHandler;
                _convertedSavedSettingsEventHandlers.Remove(value);
            }
        }

        #endregion EVENTS
    }
}
