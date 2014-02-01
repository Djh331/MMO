using MMO.Photon.Application;
using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Photon.Server
{
    class PhotonServerPeer : ServerPeerBase
    {
        private readonly PhotonServerHandlerList _handlerList;
        protected readonly PhotonApplication Server;

        public PhotonServerPeer(IRpcProtocol protocol, IPhotonPeer photonPeer, PhotonServerHandlerList handlerList, PhotonApplication application) : base(protocol, photonPeer)
        {
            _handlerList = handlerList;
            Server = application;
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            _handlerList.HandeMessage(new PhotonRequest(operationRequest.OperationCode, operationRequest.Parameters.ContainsKey(Server.SubCodeParameterKey) ? (int?)Convert.ToInt32(operationRequest.Parameters[Server.SubCodeParameterKey]) : null, operationRequest.Parameters), this);
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            _handlerList.HandeMessage(new PhotonResponse(operationResponse.OperationCode, operationResponse.Parameters.ContainsKey(Server.SubCodeParameterKey) ? (int?)Convert.ToInt32(operationResponse.Parameters[Server.SubCodeParameterKey]) : null, operationResponse.Parameters), this);
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            _handlerList.HandeMessage(new PhotonEvent(eventData.Code, eventData.Parameters.ContainsKey(Server.SubCodeParameterKey) ? (int?)Convert.ToInt32(eventData.Parameters[Server.SubCodeParameterKey]) : null, eventData.Parameters), this);
        }
    }
}
