using Prism.Mvvm;

namespace MyCSharpLib.Wpf
{
    internal class ControlStrings : BindableBase, IControlsStrings
    {
        public string WrongStringsTypeExceptionMessage { get; set; } = "...";

        public string Id { get; set; } = "Id";
        public string Time { get; set; } = "Time";
        public string Source { get; set; } = "Source";
        public string Tag { get; set; } = "Tag";
        public string EventType { get; set; } = "Event type";
        public string Title { get; set; } = "Title";
        public string Payload { get; set; } = "Payload";

        public string OperationStack { get; set; } = "Operation stack";
        public string TimeStamp { get; set; } = "Time stamp";
        public string ProcessId { get; set; } = "Process id";
        public string ThreadId { get; set; } = "Thread id";
        public string CallStack { get; set; } = "Call stack";

        public string Header { get; set; } = "Header";
        public string Body { get; set; } = "Body";
        public string Footer { get; set; } = "Footer";

        public string Filter { get; set; } = "Filter";

        public string Everything { get; set; } = "All";
        public string Critical { get; set; } = "Critical";
        public string Error { get; set; } = "Error";
        public string Warning { get; set; } = "Warning";
        public string Information { get; set; } = "Information";
        public string Verbose { get; set; } = "Verbose";
        public string Start { get; set; } = "Start";
        public string Stop { get; set; } = "Stop";
        public string Suspend { get; set; } = "Suspend";
        public string Resume { get; set; } = "Resume";
        public string Transfer { get; set; } = "Transfer";
        public string Search { get; set; } = "Search";
    }
}
