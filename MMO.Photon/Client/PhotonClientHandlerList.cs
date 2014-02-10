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
                    Log.WarnFormat("Failed to register handler {0} for type {1}:{2}", h.GetType().Name, h.Type, h.Code);
                }
            }
        }

        /// <summary>
        /// Adds a handler to our list
        /// </summary>
        /// <param name="handler">Handler to add.</param>
        /// <returns>If the handler is added to our list, or already exists in the list, returns <b>true</b>.</returns>
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
                else if (!_requestHandlerList.ContainsKey(handler.Code)) //! here???
                {
                    _requestHandlerList.Add(handler.Code, handler);
                    registered = true;
                }
                else
                {
                    Log.ErrorFormat("RequestHandler list already contains handler for {0} - cannot add {1}", handler.Code, handler.GetType().Name);
                    /*if (handler.SubCode.HasValue)
                        Log.ErrorFormat("The handler subcode: {0}", handler.SubCode.Value.ToString());
                    else
                        Log.ErrorFormat("The handler does not contain a subcode!");
                    Log.Error("List (size="+_requestHandlerList.Count.ToString()+") of registered handlers:");
                    foreach (var h in this._requestHandlerList)
                        Log.Error(h.Value.GetType().Name);*/
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
            Log.Debug("Handling message from client");
            bool handled = false;

            switch (message.Type)
            {
                case MessageType.Request:
                    if (message.SubCode.HasValue && _requestHandlerList.ContainsKey(message.SubCode.Value)) //look for specific handler
                    {
                        Log.DebugFormat("Passing message to {0} for handling...", _requestHandlerList[message.SubCode.Value]);
                        _requestHandlerList[message.SubCode.Value].HandleMessage(message, peer);
                        handled = true;
                    }
                    else if (_requestHandlerList.ContainsKey(message.Code)) //bypass and look for any handler that can handle this message type
                    {
                        Log.DebugFormat("Passing message to {0} for handling...", _requestHandlerList[message.Code]);
                        _requestHandlerList[message.Code].HandleMessage(message, peer);
                        handled = true;
                    }
                    break;
                default:
                    Log.DebugFormat("Message is of type {0}, expecting Request!", message.Type);
                    break;
            }
            return handled;
        }
    }
}
