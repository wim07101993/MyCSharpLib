using System;
using System.Collections.Generic;

namespace MyCSharpLib.Services.Telnet
{
    public class TelnetMessage
    {
        public DateTime Time { get; } = DateTime.Now;
        public string Sender { get; set; }
        public IEnumerable<byte> Content { get; set; }
    }
}
