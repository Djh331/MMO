﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMO.Photon.Server;
using MMO.Photon.Client;
using ExitGames.Logging;
using Photon.SocketServer;

namespace MMO.Photon.Application
{
    public class PhotonPeerFactory
    {
        private readonly PhotonServerPeer.Factory _serverPeerFactory;
        private readonly PhotonClientPeer.Factory _clientPeerFactory;
        private readonly PhotonConnectionCollection _subServerCollection;
        private readonly PhotonApplication _application;
        protected readonly ILogger Log;

        public PhotonPeerFactory(PhotonServerPeer.Factory serverPeerFactory, PhotonClientPeer.Factory clientPeerFactory, PhotonConnectionCollection subServerCollection, PhotonApplication application)
        {
            _serverPeerFactory = serverPeerFactory;
            _clientPeerFactory = clientPeerFactory;
            _subServerCollection = subServerCollection;
            _application = application;
            Log = _application.Log;
        }

        public PeerBase CreatePeer(InitRequest initRequest)
        {
            if (IsServerPeer(initRequest))
            {
                if (Log.IsDebugEnabled)
                    Log.DebugFormat("Recieved init request from sub server");

                return _serverPeerFactory(initRequest.Protocol, initRequest.PhotonPeer);
            }

            if (Log.IsDebugEnabled)
                Log.DebugFormat("Recieved init request from client");

            return _clientPeerFactory(initRequest.Protocol,initRequest.PhotonPeer);
        }

        public PhotonServerPeer CreatePeer(InitResponse initResponse)
        {
            var subServerPeer = _serverPeerFactory(initResponse.Protocol, initResponse.PhotonPeer);

            if (Log.IsDebugEnabled)
                Log.DebugFormat("Recieved init response from sub server");

            if (initResponse.RemotePort == _application.MasterEndPoint.Port)
            {
                _application.Register(subServerPeer);

                if (Log.IsDebugEnabled)
                    Log.DebugFormat("Registering sub server");
            }

            return subServerPeer;
        }

        protected bool IsServerPeer(InitRequest initRequest)
        {
            return _subServerCollection.IsServerPeer(initRequest);
        }
    }
}
