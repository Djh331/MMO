using MMO.Framework;
using MMO.Photon.Application;
using System;
using System.Collections.Generic;

namespace MMO.Photon.Client
{
    public class SubServerClientPeer : IClient
    {
        private readonly Guid _peerId;
        private readonly Dictionary<Type, IClientData> _clientData = new Dictionary<Type, IClientData>();
        private readonly PhotonApplication _server;

        public Guid PeerId
        {
            get
            {
                return _peerId;
            }
            set
            {
                throw new NotImplementedException("Setting a Peer ID? No-No!");
            }
        }

        public delegate SubServerClientPeer Factory(Guid peerId);

        public SubServerClientPeer(IEnumerable<IClientData> clientData, PhotonApplication application, Guid peerId)
        {
            _peerId = peerId;
            _server = application;
            foreach(var data in clientData)
            {
                _clientData.Add(data.GetType(), data);
            }
        }

        public T ClientData<T>() where T : class, IClientData
        {
            IClientData result;
            _clientData.TryGetValue(typeof(T),out result);
            if (result != null)
                return result as T;
            return null;
        }
    }
}
