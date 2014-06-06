using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMO.Framework;
using MMO.Photon.Client;
using MMO.Photon.Server;
using ExitGames.Logging;
using Photon.SocketServer;

namespace MMO.Photon.Application
{
    public abstract class PhotonConnectionCollection : IConnectionCollection<PhotonServerPeer, PhotonClientPeer>
    {
        protected ILogger Log = LogManager.GetCurrentClassLogger();
        public Dictionary<Guid, PhotonClientPeer> Clients { get; protected set; }
        public Dictionary<Guid, PhotonServerPeer> Servers { get; protected set; }

        public PhotonConnectionCollection()
        {
            Servers = new Dictionary<Guid, PhotonServerPeer>();
            Clients = new Dictionary<Guid, PhotonClientPeer>();
        }

        public void OnConnect(PhotonServerPeer serverPeer)
        {
            Log.DebugFormat("Server {0} attempting to connect.", serverPeer.ApplicationName);

            if (!serverPeer.ServerId.HasValue)
                throw new InvalidOperationException("Server ID cannot be null!");

            Guid id = serverPeer.ServerId.Value;

            lock (this)
            {
                PhotonServerPeer peer;
                if (Servers.TryGetValue(id, out peer))
                {
                    peer.Disconnect();
                    Servers.Remove(id);
                    Disconnect(peer);
                }

                Servers.Add(id, serverPeer);
                Log.Warn("Sending to connect to new server peer");
                Connect(serverPeer);

                ResetServers();
            }
        }

        public void OnDisconnect(PhotonServerPeer serverPeer)
        {
            if (!serverPeer.ServerId.HasValue)
            {
                //try to invoke a reset here for failures?
                Disconnect(serverPeer);
                throw new InvalidOperationException("Server ID cannot be null!");
            }

            lock(this)
            {
                PhotonServerPeer peer;

                Guid id = serverPeer.ServerId.Value;
                if (!Servers.TryGetValue(id, out peer))
                    return;

                if (peer == serverPeer)
                {
                    Servers.Remove(id);
                    Disconnect(peer);
                    ResetServers();
                }
            }
        }

        public void OnClientConnect(PhotonClientPeer clientPeer)
        {
            ClientConnect(clientPeer);
            Clients.Add(clientPeer.PeerID, clientPeer);
        }

        public void OnClientDisconnect(PhotonClientPeer clientPeer)
        {
            ClientDisconnect(clientPeer);
            Clients.Remove(clientPeer.PeerID);
        }

        public PhotonServerPeer GetServerByType(int serverType, params object[] additional)
        {
            return OnGetServerByType(serverType, additional);
        }

        public abstract void Disconnect(PhotonServerPeer serverPeer);
        public abstract void Connect(PhotonServerPeer serverPeer);
        public abstract void ClientConnect(PhotonClientPeer clientPeer);
        public abstract void ClientDisconnect(PhotonClientPeer clientPeer);

        public abstract void ResetServers();

        public abstract bool IsServerPeer(InitRequest initRequest);
        public abstract PhotonServerPeer OnGetServerByType(int serverType, params object[] additional);
        public abstract void DisconnectAll();
    }
}
