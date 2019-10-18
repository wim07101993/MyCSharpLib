using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Injection;
using WSharp.Logging.Loggers;

namespace WSharp.Logging
{
    public class LogDispatcherFactory : ILogDispatcherFactory
    {
        #region FIELDS

        private readonly IUnityContainer _unityContainer;
        private readonly List<Type> _loggerTypes = new List<Type>();

        private ILogDispatcher _logDisptacherIntance;
        private bool _useSingleton;

        #endregion FIELDS

        #region CONSTRUCTOR

        public LogDispatcherFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            LogDispatcherType = typeof(LogDispatcher);
            _useSingleton = true;
        }

        #endregion CONSTRUCTOR

        #region PROPERTIES

        public IEnumerable<Type> LoggerTypes => _loggerTypes;

        public Type LogDispatcherType { get; private set; }

        #endregion PROPERTIES

        #region METHODS

        public ILogDispatcherFactory RegisterLogDispatcherType<T>() where T : ILogDispatcher
        {
            _logDisptacherIntance = null;
            LogDispatcherType = typeof(T);
            _useSingleton = false;
            return this;
        }

        public ILogDispatcherFactory RegisterLogDispatcherInstance<T>(T instance) where T : ILogDispatcher
        {
            _logDisptacherIntance = instance;
            LogDispatcherType = typeof(T);
            _useSingleton = true;
            return this;
        }

        public ILogDispatcherFactory RegisterLogDispatcherSingleton<T>() where T : ILogDispatcher
        {
            _logDisptacherIntance = null;
            LogDispatcherType = typeof(T);
            _useSingleton = true;
            return this;
        }

        public ILogDispatcher Resolve()
        {
            if (LogDispatcherType == null)
                throw new InvalidOperationException("Type of log dispatcher not set");

            if (_useSingleton && _logDisptacherIntance != null)
                return _logDisptacherIntance;

            foreach (var constructor in LogDispatcherType.GetConstructors())
            {
                try
                {
                    var constructorParams = constructor
                        .GetParameters()
                        .Select(x => _unityContainer.Resolve(x.ParameterType))
                        .ToArray();

                    _logDisptacherIntance = constructor.Invoke(constructorParams) as ILogDispatcher;
                    if (_logDisptacherIntance != null)
                        break;
                }
                catch
                {
                    // could not construct, try another one
                }
            }

            if (_logDisptacherIntance == null)
                throw new InvalidOperationException("Could not resolve the log dispatcher");

            foreach (var t in _loggerTypes)
                _logDisptacherIntance.Add((ILogger)_unityContainer.Resolve(t));

            return _logDisptacherIntance;
        }

        public ILogDispatcherFactory RegisterType<TInterface, TImplementation>(params InjectionMember[] injectionMembers)
            where TInterface : ILogger
            where TImplementation : TInterface
        {
            _unityContainer.RegisterType<TInterface, TImplementation>(injectionMembers);
            _loggerTypes.Add(typeof(TInterface));
            return this;
        }

        public ILogDispatcherFactory RegisterInstance<TInterface, TImplementation>(TImplementation listener)
            where TInterface : ILogger
            where TImplementation : TInterface
        {
            _unityContainer.RegisterInstance<TInterface>(listener);
            _loggerTypes.Add(typeof(TInterface));
            return this;
        }

        public ILogDispatcherFactory RegisterSingleton<TInterface, TImplementation>(params InjectionMember[] injectionMembers)
            where TInterface : ILogger
            where TImplementation : TInterface
        {
            _unityContainer.RegisterSingleton<TInterface, TImplementation>(injectionMembers);
            _loggerTypes.Add(typeof(TInterface));
            return this;
        }

        #endregion METHODS
    }
}