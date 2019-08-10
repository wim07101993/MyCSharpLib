namespace MyCSharpLib.Wpf
{
    internal class ControlStrings : Services.Strings, IControlsStrings
    {
        private string _id = "Id";
        private string _time = "Time";
        private string _source = "Source";
        private string _tag = "Tag";
        private string _eventType = "Event type";
        private string _title = "Title";
        private string _payload = "Payload";
        private string _operationStack = "Operation stack";
        private string _timeStamp = "Time stamp";
        private string _processId = "Process id";
        private string _threadId = "Thread id";
        private string _callStack = "Call stack";
        private string _header = "Header";
        private string _body = "Body";
        private string _footer = "Footer";
        private string _filter = "Filter";
        private string _search = "Search";
        private string _clear = "Clear";
        private string _end = "End";
        private string _startDate = "Start date";
        private string _startTime = "Start time";
        private string _endDate = "End date";
        private string _endTime = "End time";

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }
        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }
        public string Tag
        {
            get => _tag;
            set => SetProperty(ref _tag, value);
        }
        public string EventType
        {
            get => _eventType;
            set => SetProperty(ref _eventType, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string Payload
        {
            get => _payload;
            set => SetProperty(ref _payload, value);
        }
        public string OperationStack
        {
            get => _operationStack;
            set => SetProperty(ref _operationStack, value);
        }
        public string TimeStamp
        {
            get => _timeStamp;
            set => SetProperty(ref _timeStamp, value);
        }
        public string ProcessId
        {
            get => _processId;
            set => SetProperty(ref _processId, value);
        }
        public string ThreadId
        {
            get => _threadId;
            set => SetProperty(ref _threadId, value);
        }
        public string CallStack
        {
            get => _callStack;
            set => SetProperty(ref _callStack, value);
        }
        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }
        public string Body
        {
            get => _body;
            set => SetProperty(ref _body, value);
        }
        public string Footer
        {
            get => _footer;
            set => SetProperty(ref _footer, value);
        }
        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }
        public string Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }
        public string Clear
        {
            get => _clear;
            set => SetProperty(ref _clear, value);
        }

        public string End
        {
            get => _end;
            set => SetProperty(ref _end, value);
        }
        public string StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }
        public string StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }
        public string EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }
        public string EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }

        #region trace event types

        private string _critical = "Critical";
        private string _error = "Error";
        private string _warning = "Warning";
        private string _information = "Information";
        private string _verbose = "Verbose";
        private string _start = "Start";
        private string _stop = "Stop";
        private string _suspend = "Suspend";
        private string _resume = "Resume";
        private string _transfer = "Transfer";
        private string _everything = "Everything";

        public string Everything
        {
            get => _everything;
            set => SetProperty(ref _everything, value);
        }
        public string Critical
        {
            get => _critical;
            set => SetProperty(ref _critical, value);
        }
        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }
        public string Warning
        {
            get => _warning;
            set => SetProperty(ref _warning, value);
        }
        public string Information
        {
            get => _information;
            set => SetProperty(ref _information, value);
        }
        public string Verbose
        {
            get => _verbose;
            set => SetProperty(ref _verbose, value);
        }
        public string Start
        {
            get => _start;
            set => SetProperty(ref _start, value);
        }
        public string Stop
        {
            get => _stop;
            set => SetProperty(ref _stop, value);
        }
        public string Suspend
        {
            get => _suspend;
            set => SetProperty(ref _suspend, value);
        }
        public string Resume
        {
            get => _resume;
            set => SetProperty(ref _resume, value);
        }
        public string Transfer
        {
            get => _transfer;
            set => SetProperty(ref _transfer, value);
        }

        #endregion trace even types
    }
}
