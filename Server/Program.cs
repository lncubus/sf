using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;

using ClientLibrary;
using ServerLibrary;

namespace Server
{
    class MainClass
    {
        public static void Pause()
        {
            Console.Error.WriteLine ("Press any key to stop...");
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(150);
            }
        }

        public static ServiceHost CreateHost()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport, true)
                {
                    MaxBufferPoolSize = Int32.MaxValue,
                    MaxBufferSize = Int32.MaxValue,
                    MaxConnections = Int32.MaxValue,
                    MaxReceivedMessageSize = Int32.MaxValue,
                    CloseTimeout = new TimeSpan(0, 0, 1),
                    OpenTimeout = new TimeSpan(0, 0, 3),
                    ReceiveTimeout = new TimeSpan(0, 0, 2),
                    SendTimeout = new TimeSpan(0, 0, 2),
                    ListenBacklog = 50,
                };
            var address = new Uri (Properties.Settings.Default.Server);
            var host = new ServiceHost (typeof(SpaceConnection));
            host.AddServiceEndpoint
            (
                implementedContract: typeof(ISpaceConnection),
                binding: binding,
                address: address
            );
            return host;
        }

        public static void Main(string[] args)
        {
            try
            {
                Trace.WriteLine("Starting...");
                ServiceHost host = CreateHost();
                host.Open();
                Trace.WriteLine("Started.");
                Pause();
                Trace.WriteLine("Stopping.");
                host.Close();
                Trace.WriteLine("Stopped.");
            }
            catch (Exception ex)
            {
				Console.Error.WriteLine (ex);
            }
        }
    }
}
