using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    /// <summary>
    /// Interface for a list of handlers a server has.
    /// </summary>
    /// <typeparam name="T">The type of peer the handlers in this list handle from.</typeparam>
    public interface IHandlerList<T>
    {
        /// <summary>
        /// Attempt to register a new handler into this list.
        /// </summary>
        /// <param name="handler">The handler to register.</param>
        /// <returns><b>True</b> if the handler was successfully added. <b>False</b> otherwise.</returns>
        bool RegisteredHandler(IHandler<T> handler);
        /// <summary>
        /// Handles any message from a client or server.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <param name="peer">The peer (Client or Server) that sent this message.</param>
        /// <returns><b>True</b> if the handler successfully handled the message. <b>False</b> otherwise; E.G. an error or not handler of that type registered in the list.</returns>
        bool HandleMessage(IMessage message, T peer);
    }
}
