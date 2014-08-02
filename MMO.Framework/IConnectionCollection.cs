using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    /// <summary>
    /// Interface for a list of connections with clients and/or other servers.
    /// </summary>
    /// <typeparam name="Server">Type (class) of server to handle connections with.</typeparam>
    /// <typeparam name="Client">Type (class) of client to handle connections with.</typeparam>
    public interface IConnectionCollection<Server, Client>
    {
        /// <summary>
        /// Called when a server connects.
        /// </summary>
        /// <param name="serverPeer">The connecting server.</param>
        void OnConnect(Server serverPeer);
        /// <summary>
        /// Called when a server disconnects.
        /// </summary>
        /// <param name="serverPeer">The disconnecting server.</param>
        void OnDisconnect(Server serverPeer);
        /// <summary>
        /// Called when a client connects.
        /// </summary>
        /// <param name="clientPeer">The connecting client.</param>
        void OnClientConnect(Client clientPeer);
        /// <summary>
        /// Called when a client disconnects.
        /// </summary>
        /// <param name="clientPeer">The disconnecting client.</param>
        void OnClientDisconnect(Client clientPeer);
        /// <summary>
        /// Get a server out of this connection collection (list)
        /// </summary>
        /// <param name="serverType">The int casted ServerType enum of the server you want.</param>
        /// <param name="additional">Any additional packaged data your collection may need.</param>
        /// <returns></returns>
        Server GetServerByType(int serverType, params object[] additional);
    }
}
