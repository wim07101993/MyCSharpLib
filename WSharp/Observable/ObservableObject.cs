using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WSharp.Observable
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
            => Set(ref storage, value, null, propertyName);

        protected virtual bool Set<T>(ref T storage, T value, Action action, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            action?.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged(string propertyName)
            => RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected virtual void RaisePropertyChanged(PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(this, e);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
