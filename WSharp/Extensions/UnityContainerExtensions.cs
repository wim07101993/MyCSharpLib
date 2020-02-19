using Unity;

using WSharp.Files;
using WSharp.Logging;
using WSharp.Logging.Loggers;
using WSharp.Serialization;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for an <see cref="IUnityContainer"/>.</summary>
    public static class UnityContainerExtensions
    {
        /// <summary>
        ///     This method registers interfaces from the WSharp library:
        ///     <list type="bullet">
        ///         <item><see cref="IFileService"/> -&gt; <see cref="FileService"/>;</item>
        ///         <item><see cref="IConsoleLogger"/> -&gt; <see cref="ConsoleLogger"/>;</item>
        ///         <item><see cref="IFileLogger"/> -&gt; <see cref="FileLogger"/>;</item>
        ///         <item><see cref="IMemoryLogger"/> -&gt; <see cref="MemoryLogger"/>;</item>
        ///         <item><see cref="ITraceLogger"/> -&gt; <see cref="TraceLogger"/>;</item>
        ///         <item><see cref="ILogDispatcherFactory"/> -&gt; <see cref="LogDispatcherFactory"/>;</item>
        ///         <item><see cref="ISerializer"/> -&gt; <see cref="JsonSerializer"/>;</item>
        ///         <item><see cref="IDeserializer"/> -&gt; <see cref="JsonSerializer"/>;</item>
        ///         <item><see cref="ISerializerDeserializer"/> -&gt; <see cref="JsonSerializer"/>;</item>
        ///     </list>
        ///     When using this method you might need to register:
        ///     <list type="bullet">
        ///         <item>an instance of <see cref="IFileServiceSettings"/>: used by the <see cref="FileService"/>.</item>
        ///         <item><see cref="IDebugLogger"/>.</item>
        ///         <item>
        ///             <see cref="ILogger"/>: usefull to set a default logger (can be the <see cref="ILogDispatcher"/>).
        ///         </item>
        ///         <item><see cref="ILogger"/> s to the <see cref="ILogDispatcherFactory"/>.</item>
        ///     </list>
        /// </summary>
        /// <param name="unityContainer">The unity container to register the dependencies on.</param>
        /// <returns>
        ///     The unity container to register the dependencies on with the registered dependencies.
        /// </returns>
        public static IUnityContainer RegisterWSharp(this IUnityContainer unityContainer)
        {
            return unityContainer
                .RegisterSingleton<IFileService, FileService>()
                .RegisterSingleton<IConsoleLogger, ConsoleLogger>()
                .RegisterSingleton<IFileLogger, FileLogger>()
                .RegisterSingleton<IMemoryLogger, MemoryLogger>()
                .RegisterSingleton<ITraceLogger, TraceLogger>()
                .RegisterSingleton<ILogDispatcherFactory, LogDispatcherFactory>()
                .RegisterFactory<ILogDispatcher>(x => x.Resolve<ILogDispatcherFactory>().Resolve())
                .RegisterSingleton<ISerializer, JsonSerializer>()
                .RegisterSingleton<IDeserializer, JsonSerializer>()
                .RegisterSingleton<ISerializerDeserializer, JsonSerializer>();
        }
    }
}
