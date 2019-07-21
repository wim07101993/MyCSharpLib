using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    public abstract class AStringsProvider<T> : BindableBase, IStringsProvider<T>, IStringsProvider where T : IStrings, new()
    {
        #region FIELDS

        private bool _isFetching;
        private bool _isSaving;

        private string _language;
       
        #endregion FIELDS


        #region PROPERTIES

        public T Strings { get; } = new T();
        IStrings IStringsProvider.Strings => Strings;

        public bool IsFetching
        {
            get => _isFetching;
            private set => SetProperty(ref _isFetching, value);

        }
        public bool IsSaving
        {
            get => _isSaving;
            private set => SetProperty(ref _isSaving, value);
        }

        public abstract string[] Languages { get; protected set; }
        public virtual string Language
        {
            get => _language;
            set
            {
                if (!SetProperty(ref _language, value))
                    return;

                _ = FetchStringsAsync(_language);
            }
        }

        #endregion PROPERTIES


        #region METHODS

        protected abstract Task<T> InternalFetchStringsAsync(string language);
        public async Task FetchStringsAsync(string language)
        {
            IsFetching = true;
            T strings;

            strings = await InternalFetchStringsAsync(language);
            CopyStringsFrom(strings);

            IsFetching = false;

            RaiseFetchedStringsEvent();

            await SaveStringsAsync(Strings, language);
        }

        protected abstract Task InternalSaveStringsAsync(T strings, string language);
        public async Task SaveStringsAsync(T strings, string language)
        {
            IsSaving = true;
            await InternalSaveStringsAsync(strings, language);
            IsSaving = false;

            RaiseSavedStringsEvent();
        }

        public async Task SaveStringsAsync(IStrings strings, string language)
        {
            if (!(strings is T s))
                throw new ArgumentException(Strings.WrongStringsTypeExceptionMessage, nameof(strings));

            await SaveStringsAsync(s, language);
        }

        protected virtual void RaiseFetchedStringsEvent() => FetchedStrings?.Invoke(this, Strings);
        protected virtual void RaiseSavedStringsEvent() => SavedStrings?.Invoke(this, Strings);

        protected void CopyStringsFrom(T strings)
        {
            var properties = typeof(T)
              .GetProperties()
              .Where(x => x.CanWrite && x.CanRead);

            foreach (var property in properties)
                property.SetValue(Strings, property.GetValue(strings));
        }

        protected virtual void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var settings = (T)sender;
            switch (e.PropertyName)
            {
                case nameof(StringsSettings.LanguagesDirectory):
                case nameof(StringsSettings.Language):
                    _ = FetchStringsAsync(settings.Language);
                    break;
            }
        }

        #endregion METHODS


        #region EVENTS

        public event EventHandler<T> FetchedStrings;

        private readonly Dictionary<EventHandler<IStrings>, EventHandler<T>> _convertedFetchedStringsEventHandlers = new Dictionary<EventHandler<IStrings>, EventHandler<T>>();
        event EventHandler<IStrings> IStringsProvider.FetchedStrings
        {
            add
            {
                var convertedEventHandler = new EventHandler<T>((sender, e) => value(sender, e));
                if (!_convertedFetchedStringsEventHandlers.ContainsKey(value))
                {
                    _convertedFetchedStringsEventHandlers.Add(value, convertedEventHandler);
                    FetchedStrings += convertedEventHandler;
                }
            }

            remove
            {
                if (!_convertedFetchedStringsEventHandlers.ContainsKey(value))
                    return;

                var convertedEventHandler = _convertedFetchedStringsEventHandlers[value];
                FetchedStrings -= convertedEventHandler;
                _convertedFetchedStringsEventHandlers.Remove(value);
            }
        }

        public event EventHandler<T> SavedStrings;

        private readonly Dictionary<EventHandler<IStrings>, EventHandler<T>> _convertedSavedStringsEventHandlers = new Dictionary<EventHandler<IStrings>, EventHandler<T>>();
        event EventHandler<IStrings> IStringsProvider.SavedStrings
        {
            add
            {
                var convertedEventHandler = new EventHandler<T>((sender, e) => value(sender, e));
                if (!_convertedSavedStringsEventHandlers.ContainsKey(value))
                {
                    _convertedSavedStringsEventHandlers.Add(value, convertedEventHandler);
                    SavedStrings += convertedEventHandler;
                }
            }

            remove
            {
                if (!_convertedSavedStringsEventHandlers.ContainsKey(value))
                    return;

                var convertedEventHandler = _convertedSavedStringsEventHandlers[value];
                SavedStrings -= convertedEventHandler;
                _convertedSavedStringsEventHandlers.Remove(value);
            }
        }

        #endregion EVENTS
    }
}
