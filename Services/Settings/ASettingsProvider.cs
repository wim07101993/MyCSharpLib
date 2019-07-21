using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    public abstract class ASettingsProvider<T> : ISettingsProvider<T>, ISettingsProvider where T : Settings, new()
    {
        #region PROPERTIES

        public virtual T Settings { get; } = new T();
        Settings ISettingsProvider.Settings => Settings;

        public bool IsFetching { get; private set; }
        public bool IsSaving { get; private set; }

        #endregion PROPERTIES


        #region METHODS

        protected abstract Task<T> InternalFetchSettingsAsync();
        public async Task FetchSettingsAsync()
        {
            IsFetching = true;

            T settings = await InternalFetchSettingsAsync();
            CopySettingsFrom(settings);

            IsFetching = false;

            RaiseFetchedSettingsEvent();

            await SaveSettingsAsync(Settings);
        }

        protected abstract Task InternalSaveSettingsAsync(T settings);

        public async Task SaveSettingsAsync(T settings)
        {
            IsSaving = true;

            CopySettingsFrom(settings);
            await InternalSaveSettingsAsync(Settings);

            IsSaving = false;

            RaiseSavedSettingsEvent();
        }

        public async Task SaveSettingsAsync(Settings settings)
        {
            if (!(settings is T s))
                throw new ArgumentException("Invalid settings type.");

            await SaveSettingsAsync(s);
        }

        protected virtual void RaiseFetchedSettingsEvent() => FetchedSettings?.Invoke(this, Settings);
        protected virtual void RaiseSavedSettingsEvent() => SavedSettings?.Invoke(this, Settings);

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

        public event EventHandler<T> FetchedSettings;

        private readonly Dictionary<EventHandler<Settings>, EventHandler<T>> _convertedFetchedSettingsEventHandlers = new Dictionary<EventHandler<Settings>, EventHandler<T>>();
        event EventHandler<Settings> ISettingsProvider.FetchedSettings
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

        public event EventHandler<T> SavedSettings;

        private readonly Dictionary<EventHandler<Settings>, EventHandler<T>> _convertedSavedSettingsEventHandlers = new Dictionary<EventHandler<Settings>, EventHandler<T>>();
        event EventHandler<Settings> ISettingsProvider.SavedSettings
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
