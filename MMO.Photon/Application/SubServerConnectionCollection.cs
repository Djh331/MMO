using System;
using System.Collections.Generic;
using MMO.Photon.Server;
using MMO.Photon.Client;
using Photon.SocketServer;

namespace MMO.Photon.Application
{
    public class SubServerConnectionCollection : PhotonConnectionCollection
    {
        public new Dictionary<Guid, SubServerClientPeer> Clients { get; protected set; }

        public SubServerConnectionCollection()
        {
            Clients = new Dictionary<Guid, SubServerClientPeer>();
        }

        public override void Disconnect(PhotonServerPeer serverPeer)
        {
        }

        public override void Connect(PhotonServerPeer serverPeer)
        {
        }

        public override void ClientConnect(PhotonClientPeer clientPeer)
        {
            throw new NotImplementedException();
        }

        public override void ClientDisconnect(PhotonClientPeer clientPeer)
        {
            throw new NotImplementedException();
        }

        public void ClientConnection(SubServerClientPeer clientPeer)
        {
        }

        public override void ResetServers()
        {
        }

        public override bool IsServerPeer(InitRequest initRequest)
        {
            return false;
        }

        public override PhotonServerPeer OnGetServerByType(int serverType)
        {
            throw new NotImplementedException();
        }

        public override void DisconnectAll()
        {
            foreach (var p in Servers)
                p.Value.Disconnect();
        }
    }
}
