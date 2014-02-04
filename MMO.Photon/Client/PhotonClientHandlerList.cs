using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Logging;
using MMO.Framework;

namespace MMO.Photon.Client
{
    public class PhotonClientHandlerList
    {
        protected readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, PhotonClientHandler> _requestHandlerList;

        public PhotonClientHandlerList(IEnumerable<PhotonClientHandler> handlers)
        {
            _requestHandlerList = new Dictionary<int, PhotonClientHandler>();

            foreach (var h in handlers)
            {
                if (!RegisterHandler(h))
                {
                    Log.WarnFormat("Attempted to register handler {0} for type {1}:{2}", h.GetType().Name, h.Type, h.Code);
                }
            }
        }

        /// <summary>
        /// Adds a handler to our list
        /// </summary>
        /// <param name="handler">Handler to add.</param>
        /// <returns>If the handler is added to our list, returns <b>true</b>.</returns>
        public bool RegisterHandler(PhotonClientHandler handler)
        {
            var registered = false;

            if ((handler.Type & MessageType.Request) == MessageType.Request)
            {
                if (handler.SubCode.HasValue && !_requestHandlerList.ContainsKey(handler.SubCode.Value))
                {
                    _requestHandlerList.Add(handler.SubCode.Value, handler);
                    registered = true;
                }
                else if (!_requestHandlerList.ContainsKey(handler.Code))
                {
                    _requestHandlerList.Add(handler.Code, handler);
                    registered = true;
                }
                else
                {
                    Log.ErrorFormat("RequestHandler list already contains handler for {0} - cannot add {1}", handler.Code, handler.GetType().Name);
                }
            }

            return registered;
        }

        /// <summary>
        /// Tells the handler how to read the request.
        /// </summary>
        /// <param name="message">IMessage to read.</param>
        /// <param name="peer">Client sending the message.</param>
        /// <returns>If the message is handled, returns <b>true</b>.</returns>
        public bool HandleMessage(IMessage message, PhotonClientPeer peer)
        {
            bool handled = false;

            switch (message.Type)
            {
                case MessageType.Request:
                    if (message.SubCode.HasValue && _requestHandlerList.ContainsKey(message.SubCode.Value))
                    {
                        _requestHandlerList[message.SubCode.Value].HandleMessage(message, peer);
                        handled = true;
                    }
                    else if (!message.SubCode.HasValue && _requestHandlerList.ContainsKey(message.Code))
                    {
                        _requestHandlerList[message.Code].HandleMessage(message, peer);
                        handled = true;
                    }
                    break;
            }
            return handled;
        }
    }
}
