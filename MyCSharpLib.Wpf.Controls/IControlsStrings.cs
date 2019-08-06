using MyCSharpLib.Services;

namespace MyCSharpLib.Wpf
{
    public interface IControlsStrings : IStrings
    {
        string Id { get; set; }
        string Time { get; set; }
        string Source { get; set; }
        string Tag { get; set; }
        string EventType { get; set; }
        string Title { get; set; }
        string Payload { get; set; }

        string OperationStack { get; set; }
        string TimeStamp { get; set; }
        string ProcessId { get; set; }
        string ThreadId { get; set; }
        string CallStack { get; set; }
    }
}
