using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Logging;
using System.Net;
using MMO.Photon.Server;
using System.Threading;
using MMO.Framework;
using Photon.SocketServer;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using System.IO;
using Autofac;
using MMO.Photon.Client;
using Photon.SocketServer.ServerToServer;

namespace MMO.Photon.Application
{
    public abstract class PhotonApplication : ApplicationBase
    {
        public abstract byte SubCodeParameterKey { get; }
        private PhotonConnectionCollection _connectionCollection { get; set; }
        public static readonly Guid ServerId = Guid.NewGuid();
        protected static readonly ILogger Log = ExitGames.Logging.LogManager.GetCurrentClassLogger();

        public abstract IPEndPoint MasterEndPoint { get; }
        public abstract int? TcpPoint { get; }
        public abstract int? UdpPort { get; }
        public abstract IPAddress PublicIpAddress { get; }
        public abstract int ServerType { get; }

        protected abstract int ConnectRetryIntervalSeconds { get; }
        protected abstract bool ConnectsToMaster { get; }

        private static PhotonServerPeer _masterPeer;
        private byte _isReconnecting;
        private Timer _retry;

        private PhotonPeerFactory _factory;
        private IEnumerable<IBackgroundThread> _backgroundThreads;

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return _factory.CreatePeer(initRequest);
        }

        protected override void Setup()
        {
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["LogFileName"] = ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(BinaryPath, "log4net.config")));

            var builder = new ContainerBuilder();

            Initialize(builder);

            var container = builder.Build();

            _factory = container.Resolve<PhotonPeerFactory>();
            _connectionCollection = container.Resolve<PhotonConnectionCollection>();
            _backgroundThreads = container.Resolve<IEnumerable<IBackgroundThread>>();

            ResolveParameters(container);

            foreach (var backgroundThread in _backgroundThreads)
            {
                backgroundThread.Setup();
                ThreadPool.QueueUserWorkItem(backgroundThread.Run);
            }

            if (ConnectsToMaster)
                ConnectToMaster();
        }

        protected void Initialize(ContainerBuilder builder)
        {
            builder.RegisterType<PhotonServerPeer>();
            builder.RegisterType<PhotonPeerFactory>();
            builder.RegisterType<PhotonClientPeer>();
            builder.RegisterType<PhotonClientHandlerList>();
            builder.RegisterType<PhotonServerHandlerList>();
            RegisterContainerObjects(builder);
        }

        protected override void TearDown()
        {
        }

        protected override void OnStopRequested()
        {
            foreach (var backgroundThread in _backgroundThreads)
                backgroundThread.Stop();

            if (_connectionCollection != null)
                _connectionCollection.DisconnectAll();

            base.OnStopRequested();
        }

        public void ConnectToMaster()
        {
            if (!ConnectToServer(MasterEndPoint, "Proxy", "Proxy"))
            //if(!ConnectToServerUdp(MasterEndPoint, "Proxy", "Proxy",0,1200))
            {
                Log.Warn("Master connection refused!");
                return;
            }

            if (Log.IsDebugEnabled)
                Log.DebugFormat(_isReconnecting == 0 ? "Connecting to master at {0}" : "Reconnecting to master at {0}", MasterEndPoint);
        }

        protected override void OnServerConnectionFailed(int errorCode, string errorMessage, object state)
        {
            if (_isReconnecting == 0)
                Log.ErrorFormat("Master connection failed with error {0} : {1}", errorCode, errorMessage);
            else if(Log.IsDebugEnabled)
                Log.ErrorFormat("Master connection failed with error {0} : {1}", errorCode, errorMessage);

            string stateString = state as string;
            if (stateString != null && stateString.Equals("Master"))
            {
                ReconnectToMaster();
            }
        }

        public void ReconnectToMaster()
        {
            Thread.VolatileWrite(ref _isReconnecting, 1);
            _retry = new Timer(o => ConnectToMaster(), null, ConnectRetryIntervalSeconds * 1000, 0);
        }

        protected override ServerPeerBase CreateServerPeer(InitResponse initResponse, object state)
        {
            return _factory.CreatePeer(initResponse);
        }

        public T BackgroundThread<T>() where T : class
        {
            IBackgroundThread result;
            result = _backgroundThreads.ToList().Find(s => s.GetType() == typeof(T));
            if (result != null)
                return result as T;
            return null;
        }

        public TConnectionCollection ConnectionCollection<TConnectionCollection>()
            where TConnectionCollection : PhotonConnectionCollection
        {
            return _connectionCollection as TConnectionCollection;
        }

        protected abstract void RegisterContainerObjects(ContainerBuilder builder);
        protected abstract void ResolveParameters(IContainer container);
        public abstract void Register(PhotonServerPeer peer);
    }
}
