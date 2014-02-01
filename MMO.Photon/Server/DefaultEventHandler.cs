using MMO.Photon.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Photon.Server
{
    public abstract class DefaultEventHandler : PhotonServerHandler
    {
        public DefaultEventHandler(PhotonApplication application)
            : base(application)
        {
        }
    }
}
