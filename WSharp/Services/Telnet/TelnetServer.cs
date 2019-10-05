using System.Net.Sockets;
using WSharp.Services.Logging.Loggers;
using WSharp.Services.Serialization;
using Unity;
using Unity.Resolution;

namespace WSharp.Services.Telnet
{
    public class TelnetServer : ATelnetServer<ITelnetConnection>
    {
        #region FIELDS

        private readonly IUnityContainer _unityContainer;

        #endregion FIELDS


        #region CONSTRUCTOR

        public TelnetServer(IUnityContainer unityContainer, ITelnetServerSettings settings, 
            ILogger logger, ISerializerDeserializer serializerDeserializer)
            : base(settings, logger, serializerDeserializer)
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
