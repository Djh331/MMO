using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    /// <summary>
    /// The type of message being sent.
    /// Request: 
    /// Response: 
    /// Async: 
    /// </summary>
    [Flags]
    public enum MessageType
    {
        /// <summary>
        /// A request that expects a response to be sent after processing.
        /// </summary>
        Request = 0x1,
        /// <summary>
        /// A response sent after a request.
        /// </summary>
        Response = 0x2,
        /// <summary>
        /// Any 'event' message.
        /// </summary>
        Async = 0x4
    }
}
