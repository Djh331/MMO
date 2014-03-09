using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMO.Framework;
using MMO.Photon.Application;
using ExitGames.Logging;

namespace MMO.Photon.Server
{
    public abstract class PhotonServerHandler : IHandler<PhotonServerPeer>
    {
        public abstract MessageType Type { get; }
        public abstract byte Code { get; }
        public abstract int? SubCode { get; }
        protected PhotonApplication Server;
        protected ILogger Log;

        public PhotonServerHandler(PhotonApplication application)
        {
            Server = application;
            Log = Server.Log;
        }

        public bool HandleMessage(IMessage message, PhotonServerPeer serverPeer)
        {
            OnHandleMessage(message, serverPeer);
            return true;
        }

        protected abstract bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer);
    }
}
