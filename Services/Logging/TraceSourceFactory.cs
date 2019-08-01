using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity;

namespace MyCSharpLib.Services.Logging
{
    public class TraceSourceFactory : ITraceSourceFactory
    {
        private readonly IUnityContainer _unityContainer;
        private readonly List<TraceListener> _listeners = new List<TraceListener>();


        public TraceSourceFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }


        public TraceSource Resolve(string name, SourceLevels defaultLevel = SourceLevels.All)
        {
            var traceSource = new TraceSource(name, defaultLevel);
            traceSource.Listeners.AddRange(_listeners.ToArray());
            return traceSource;
        }

        public ITraceSourceFactory RegisterListener<T>() where T : ITraceListener
        {
            var instance = _unityContainer.Resolve<T>();
            _listeners.Add(new TraceListenerWrapper<T>(instance));
            return this;
        }

        public ITraceSourceFactory RegisterListener<T>(T listener) where T : ITraceListener
        {
            _listeners.Add(new TraceListenerWrapper<T>(listener));
            return this;
        }

        public ITraceSourceFactory RegisterListener(TraceListener listener) 
        {
            _listeners.Add(listener);
            return this;
        }


        private class TraceListenerWrapper<T> : TraceListener where T : ITraceListener
        {
            public TraceListenerWrapper(T listener)
            {
                Listener = listener;
            }

            public T Listener { get;}


            public override void Close() => Listener.Close();
            public override void Flush() => Listener.Flush();

            public override void Fail(string message) => Listener.Fail(message);
            public override void Fail(string message, string detailMessage) => Listener.Fail(message, detailMessage);

            public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
                => Listener.TraceData(eventCache, source, eventType, id, data);
            public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
                => Listener.TraceData(eventCache, source, eventType, id, data);

            public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
                => Listener.TraceEvent(eventCache, source, eventType, id);
            public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
                => Listener.TraceEvent(eventCache, source, eventType, id, message);
            public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
                => Listener.TraceEvent(eventCache, source, eventType, id, format, args);

            public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
                => Listener.TraceTransfer(eventCache, source, id, message, relatedActivityId);

            public override void Write(string message) => Listener.Write(message);
            public override void Write(string message, string category) => Listener.Write(message, category);

            public override void WriteLine(string message) => Listener.WriteLine(message);
            public override void WriteLine(string message, string category) => Listener.WriteLine(message, category);

            public override void Write(object o) => Listener.Write(o);
            public override void Write(object o, string category) => Listener.Write(o, category);

            public override void WriteLine(object o) => Listener.Write(o);
            public override void WriteLine(object o, string category) => Listener.Write(o, category);

        }
    }
}
