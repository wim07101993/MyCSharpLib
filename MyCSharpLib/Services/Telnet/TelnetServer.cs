using System.Net.Sockets;
using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Services.Serialization;
using Unity;
using Unity.Resolution;

namespace MyCSharpLib.Services.Telnet
{
    public class TelnetServer : ATelnetServer<ITelnetConnection>
    {
        #region FIELDS

        private IUnityContainer _unityContainer;

        #endregion FIELDS


        #region CONSTRUCTOR

        public TelnetServer(IUnityContainer unityContainer, ITelnetServerSettings settings, 
            ILogDispatcher logDispatcher, ISerializerDeserializer serializerDeserializer)
            : base(settings, logDispatcher, serializerDeserializer)
        {
            _unityContainer = unityContainer;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        protected override string ClassName => nameof(TelnetServer);

        #endregion PROPERTIES


        #region METHODS

        protected override ITelnetConnection CreateNewConnection(TcpClient tcpClient) 
            => _unityContainer.Resolve<ITelnetConnection>(new ParameterOverride("tcpClient", tcpClient));

        #endregion METHODS
    }
}
