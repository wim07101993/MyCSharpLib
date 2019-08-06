using Prism.Mvvm;

namespace MyCSharpLib.Wpf
{
    internal class ControlStrings : BindableBase, IControlsStrings
    {
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

        public string WrongStringsTypeExceptionMessage { get; set; } = "...";
    }
}
