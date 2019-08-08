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

        string Header { get; set; }
        string Body { get; set; }
        string Footer { get; set; }

        string Filter { get; set; }

        string Everything { get; set; }
        string Critical { get; set; }
        string Error { get; set; }
        string Warning { get; set; }
        string Information { get; set; }
        string Verbose { get; set; }
        string Start { get; set; }
        string Stop { get; set; }
        string Suspend { get; set; }
        string Resume { get; set; }
        string Transfer { get; set; }

        string Search { get; set; }
        string Clear { get; set; }

        string End { get; set; }
        string StartDate { get; set; }
        string StartTime { get; set; }
        string EndDate { get; set; }
        string EndTime { get; set; }

    }
}
