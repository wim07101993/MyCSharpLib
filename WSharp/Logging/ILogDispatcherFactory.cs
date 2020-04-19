using System;
using System.Collections.Generic;

using Unity.Injection;

using WSharp.Logging.Loggers;

namespace WSharp.Logging
{
    /// <summary>Factory that produces <see cref="ILogDispatcher"/> objects.</summary>
    public interface ILogDispatcherFactory
    {
        /// <summary>Types of the registered loggers.</summary>
        IEnumerable<Type> LoggerTypes { get; }

        /// <summary>Type of the <see cref="ILogDispatcher"/> to produce.</summary>
        Type LogDispatcherType { get; }

        /// <summary>Resolves an <see cref="ILogDispatcher"/>.</summary>
        /// <returns>The resolved <see cref="ILogDispatcher"/>.</returns>
        ILogDispatcher Resolve();

        /// <summary>Sets the <see cref="ILogDispatcher"/> type to produce.</summary>
        /// <typeparam name="T">Type to register.</typeparam>
        /// <returns>The factory that registered the type.</returns>
        ILogDispatcherFactory RegisterLogDispatcherType<T>() where T : ILogDispatcher;

        /// <summary>Sets the <see cref="ILogDispatcher"/> instance to modify.</summary>
        /// <typeparam name="T">Type to register.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <returns>The factory that registered the type.</returns>
        ILogDispatcherFactory RegisterLogDispatcherInstance<T>(T instance) where T : ILogDispatcher;

        /// <summary>Registers a type of logger to have in the <see cref="ILogDispatcher"/>.</summary>
        /// <typeparam name="TInterface">Interface of the logger to register.</typeparam>
        /// <typeparam name="TImplementation">Type of the logger to register.</typeparam>
        /// <param name="injectionMembers">Parameters needed to construct the logger.</param>
        /// <returns>The factory that registered the type.</returns>
        ILogDispatcherFactory RegisterType<TInterface, TImplementation>(params InjectionMember[] injectionMembers)
            where TInterface : ILogger
            where TImplementation : TInterface;

        /// <summary>
        ///     Registers an instance of logger that each <see cref="ILogDispatcher"/> should have.
        /// </summary>
        /// <typeparam name="TInterface">Interface of the logger to register.</typeparam>
        /// <typeparam name="TImplementation">Type of the logger to register.</typeparam>
        /// <param name="instance">Instance to register.</param>
        /// <returns>The factory that registered the type.</returns>
        ILogDispatcherFactory RegisterInstance<TInterface, TImplementation>(TImplementation instance)
            where TInterface : ILogger
            where TImplementation : TInterface;

        /// <summary>Registers a singleton of a given type for the <see cref="ILogDispatcher"/>.</summary>
        /// <typeparam name="TInterface">Interface of the logger to register.</typeparam>
        /// <typeparam name="TImplementation">Type of the logger to register.</typeparam>
        /// <param name="injectionMembers">Parameters needed to construct the logger.</param>
        /// <returns>The factory that registered the type.</returns>
        ILogDispatcherFactory RegisterSingleton<TInterface, TImplementation>(params InjectionMember[] injectionMembers)
            where TInterface : ILogger
            where TImplementation : TInterface;
    }
}
