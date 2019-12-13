using System;

namespace WSharp.Events
{
    /// <summary>
    ///     Delegate used by events that pass an exception around. It can be used to pass an
    ///     exception form one thread to another.
    /// </summary>
    /// <param name="sender">The object that threw the exception.</param>
    /// <param name="exception">The thrown exception.</param>
    public delegate void ExceptionEventHandler(object sender, Exception exception);
}
