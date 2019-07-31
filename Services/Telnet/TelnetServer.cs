using System.Net.Sockets;
using System.Threading;
using MyCSharpLib.Services.Serialization;

namespace MyCSharpLib.Services.Telnet
{
    public class TelnetServer : ATelnetServer<TelnetServerConnection>
    {
        #region CONSTRUCTOR

        public TelnetServer(ITelnetServerSettings settings, ISerializerDeserializer serializerDeserializer)
            : base(settings, serializerDeserializer)
        {
        }

        #endregion CONSTRUCTOR
        

        #region METHODS

        protected override TelnetServerConnection CreateNewConnection(TcpClient tcpClient)
        {
            return new TelnetServerConnection(tcpClient, SerializerDeserializer);
        }

        #endregion METHODS
    }
}
