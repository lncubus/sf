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
            var binding = new NetTcpBinding(SecurityMode.None, true)
                {
                    //                    MaxBufferPoolSize = 0x100000 Int32.MaxValue,
                    //                    MaxBufferSize = Int32.MaxValue,
                    //                    MaxConnections = Int32.MaxValue,
                    //                    MaxReceivedMessageSize = Int32.MaxValue,
                    CloseTimeout = new TimeSpan(0, 0, 1),
                    OpenTimeout = new TimeSpan(0, 0, 3),
                    ReceiveTimeout = new TimeSpan(0, 0, 2),
                    SendTimeout = new TimeSpan(0, 0, 2),
                    ListenBacklog = 50,
                };
            var address = new Uri ("net.tcp://localhost:8090");
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
            using (var listener = new ConsoleTraceListener())
            {
                Debug.Listeners.Add(listener);
                try
                {
                    Console.WriteLine("Starting...");
                    ServiceHost host = CreateHost();
                    host.Open();
                    Console.WriteLine("Started.");
                    Pause();
                    Console.WriteLine("Stopping.");
                    host.Close();
                    Console.WriteLine("Stopped.");
                }
                finally
                {
                    Debug.Listeners.Remove(listener);
                }
            }
        }
    }
}
