using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MMO.Framework;
using ExitGames.Logging;
using MMO.Photon.Application;
using MMO.Photon.Server;

namespace MMO.Photon.Client
{
    public class PhotonClientPeer : PeerBase
    {
        protected ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly Guid _peerID;
        private readonly Dictionary<Type, IClientData> _clientData = new Dictionary<Type, IClientData>();
        private readonly PhotonApplication _server;
        private readonly PhotonClientHandlerList _handlerList;
        public PhotonServerPeer CurrentServer { get; set; }

        #region Factory Method

        public delegate PhotonClientPeer Factory(IRpcProtocol protocol, IPhotonPeer photonPeer);

        #endregion

        public PhotonClientPeer(IRpcProtocol protocol, IPhotonPeer photonPeer, IEnumerable<IClientData> clientData, PhotonClientHandlerList handlerList, PhotonApplication application)
            : base(protocol, photonPeer)
        {
            _peerID = Guid.NewGuid();
            _handlerList = handlerList;
            _server = application;
            foreach (var data in clientData)
            {
                _clientData.Add(data.GetType(), data);
            }
            _server.ConnectionCollection<PhotonConnectionCollection>().Clients.Add(_peerID, this);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            _handlerList.HandleMessage(new PhotonRequest(operationRequest.OperationCode, operationRequest.Parameters.ContainsKey(_server.SubCodeParameterKey) ? (int?)Convert.ToInt32(operationRequest.Parameters[_server.SubCodeParameterKey]) : null, operationRequest.Parameters), this);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            _server.ConnectionCollection<PhotonConnectionCollection>().OnClientDisconnect(this);
            Log.DebugFormat("Client {0} disconnected", _peerID);
        }

        public Guid PeerID
        {
            get { return _peerID; }
        }

        public T ClientData<T>() where T : class, IClientData
        {
            IClientData result;
            _clientData.TryGetValue(typeof(T), out result);
            if (result != null)
                return result as T;

            return null;
        }
    }
}
