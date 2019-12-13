namespace WSharp
{
    /// <summary>Notifier that an object has been disposed.</summary>
    public interface INotifyDisposed
    {
        /// <summary>The object has been disposed.</summary>
        event NotifyDisposedEventHandler Disposed;
    }
}
