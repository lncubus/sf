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
        public static string ServerUri;

        public static void Pause()
        {
            Console.Error.WriteLine ("Press any key to stop...");
            Console.ReadKey(true);
        }

        public static ServiceHost CreateHost()
        {
            var binding = new NetTcpBinding("netTcpBinding_Anonymous");
            var address = new Uri (ServerUri);
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
                    if (args.Length > 0)
                        ServerUri = args[0];
                    else
                        ServerUri = Properties.Settings.Default.Server;
                    Run();
                }
                finally
                {
                    Debug.Listeners.Remove(listener);
                }
            }
        }

        private static void Run()
        {
            try
            {
                Debug.WriteLine("Starting...");
                ServiceHost host = CreateHost();
                host.Open();
                Debug.WriteLine("Started.");
                Pause();
                Debug.WriteLine("Stopping.");
                host.Close();
                Debug.WriteLine("Stopped.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
    }
}
