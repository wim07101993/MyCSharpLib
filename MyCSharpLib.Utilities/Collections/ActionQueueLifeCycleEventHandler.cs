using System;
using System.Threading;

namespace MyCSharpLib.Utilities.Collections
{
    public delegate void ActionQueueLifeCycleEventHandler(ActionQueue sender, Action<CancellationToken> action);
}
