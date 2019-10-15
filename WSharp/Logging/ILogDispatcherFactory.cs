using System;
using System.Collections.Generic;
using Unity.Injection;
using WSharp.Logging.Loggers;

namespace WSharp.Logging
{
    public interface ILogDispatcherFactory
    {
        IEnumerable<Type> LoggerTypes { get; }
        Type LogDispatcherType { get; }

        ILogDispatcher Resolve();

        ILogDispatcherFactory RegisterLogDispatcherType<T>() where T : ILogDispatcher;

        ILogDispatcherFactory RegisterLogDispatcherInstance<T>(T instance) where T : ILogDispatcher;

        ILogDispatcherFactory RegisterLogDispatcherSingletond<T>() where T : ILogDispatcher;

        ILogDispatcherFactory RegisterType<TInterface, TImplementation>(params InjectionMember[] injectionMembers)
            where TInterface : ILogger
            where TImplementation : TInterface;

        ILogDispatcherFactory RegisterInstance<TInterface, TImplementation>(TImplementation listener)
            where TInterface : ILogger
            where TImplementation : TInterface;

        ILogDispatcherFactory RegisterSingleton<TInterface, TImplementation>(params InjectionMember[] injectionMembers)
            where TInterface : ILogger
            where TImplementation : TInterface;
    }
}