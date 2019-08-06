namespace MyCSharpLib.Wpf.Demo.Strings
{
    public class ApplicationStrings : Services.Strings, IControlsStrings
    {
        private string _ApplicationTitle = "Demo";
        public string ApplicationTitle
        {
            get => _ApplicationTitle;
            set => SetProperty(ref _ApplicationTitle, value);
        }


        private string _logging = "Logging";
        public string Logging
        {
            get => _logging;
            set => SetProperty(ref _logging, value);
        }

        private string _id = "Id";
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _time = "Time";
        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        private string _source = "Source";
        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        private string _tag = "Tag";
        public string Tag
        {
            get => _tag;
            set => SetProperty(ref _tag, value);
        }

        private string _eventType = "Event type";
        public string EventType
        {
            get => _eventType;
            set => SetProperty(ref _eventType, value);
        }

        private string _title = "Title";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _payload = "Payload";
        public string Payload
        {
            get => _payload;
            set => SetProperty(ref _payload, value);
        }
        
        private string _operationStack = "Operation stack";
        public string OperationStack
        {
            get => _operationStack;
            set => SetProperty(ref _operationStack, value);
        }

        private string _timeStamp = "Time stamp";
        public string TimeStamp
        {
            get => _timeStamp;
            set => SetProperty(ref _timeStamp, value);
        }

        private string _processId = "Process id";
        public string ProcessId
        {
            get => _processId;
            set => SetProperty(ref _processId, value);
        }

        private string _threadId = "Thread id";
        public string ThreadId
        {
            get => _threadId;
            set => SetProperty(ref _threadId, value);
        }

        private string _callStack = "Call stack";
        public string CallStack
        {
            get => _callStack;
            set => SetProperty(ref _callStack, value);
        }
    }
}
