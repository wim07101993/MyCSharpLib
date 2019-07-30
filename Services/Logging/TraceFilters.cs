using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyCSharpLib.Services.Logging
{
    public static class TraceFilters
    {
        public static TraceFilter NoFilter => new NoTraceFilter();

        public static TraceFilter BlockFilter => new BlockTraceFilter();

        public static TraceFilter GetSingleEventTypeFilter(TraceEventType eventType) => new SingleEventTypeTraceFilter { EventType = eventType };

        public static TraceFilter GetAndEventTypeFilter(params TraceEventType[] eventTypes) => new AndEventTypeTraceFilter { EventTypes = eventTypes };
        public static TraceFilter GetAndEventTypeFilter(IEnumerable<TraceEventType> eventTypes) => GetAndEventTypeFilter(eventTypes.ToArray());

        public static TraceFilter GetOrEventTypeFilter(params TraceEventType[] eventTypes) => new OrEventTypeTraceFilter { EventTypes = eventTypes };
        public static TraceFilter GetOrEventTypeFilter(IEnumerable<TraceEventType> eventTypes) => GetOrEventTypeFilter(eventTypes.ToArray());

        public static TraceFilter CombineAnd(params TraceFilter[] filters) => new CombinedAndTraceFilter { Filters = filters };
        public static TraceFilter CombineAnd(IEnumerable<TraceFilter> filters) => CombineAnd(filters.ToArray());

        public static TraceFilter CombineOr(params TraceFilter[] filters) => new CombinedOrTraceFilter { Filters = filters };
        public static TraceFilter CombineOr(IEnumerable<TraceFilter> filters) => CombineOr(filters.ToArray());


        #region EXTENSION METHODS

        public static TraceFilter And(this TraceFilter filter, TraceFilter filterToAdd) => CombineAnd(filter, filterToAdd);

        public static TraceFilter Or(this TraceFilter filter, TraceFilter filterToAdd) => CombineOr(filter, filterToAdd);

        #endregion EXTENSION METHODS


        #region CLASSES
#pragma warning disable RECS0016 // Bitwise operation on enum which has no [Flags] attribute

        private class NoTraceFilter : TraceFilter
        {
            public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data) 
                => true;
        }

        private class BlockTraceFilter : TraceFilter
        {
            public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
                => false;
        }

        private class SingleEventTypeTraceFilter : TraceFilter
        {
            public TraceEventType EventType { get; set; }

            public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
               => (eventType & EventType) == EventType;
        }

        private class AndEventTypeTraceFilter : TraceFilter
        {
            public TraceEventType[] EventTypes { get; set; }

            public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
               => EventTypes.All(x => (eventType & x) == x);
        }

        private class OrEventTypeTraceFilter : TraceFilter
        {
            public TraceEventType[] EventTypes { get; set; }

            public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
               => EventTypes.Any(x => (eventType & x) == x);
        }

        private class CombinedAndTraceFilter : TraceFilter
        {
            public TraceFilter[] Filters { get; set; }

            public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
                => Filters.All(x => x.ShouldTrace(cache, source, eventType, id, formatOrMessage, args, data1, data));
        }

        private class CombinedOrTraceFilter : TraceFilter
        {

            public TraceFilter[] Filters { get; set; }

            public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
                => Filters.Any(x => x.ShouldTrace(cache, source, eventType, id, formatOrMessage, args, data1, data));
        }

#pragma warning restore RECS0016 // Bitwise operation on enum which has no [Flags] attribute
        #endregion CLASSES
    }
}
