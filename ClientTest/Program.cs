using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ServiceModel;

using ClientLibrary;

namespace ClientTest
{
    class MainClass
    {
        public static string ServerUri;

        public static void Main(string[] args)
        {
            using(var listener = new ConsoleTraceListener())
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
            var binding = new NetTcpBinding("netTcpBinding_Anonymous");
            var address = new EndpointAddress (ServerUri);
            var random = new Random();
            SpaceConnectionClient client = null;

            while (true)
            {
                if (client == null || client.State != CommunicationState.Opened)
                {
                    try
                    {
                        client = new SpaceConnectionClient(binding, address);
                        string nonce = client.Connect();
                        byte[] answer = new byte[16];
                        random.NextBytes(answer);
                        string response = BitConverter.ToString(answer);
                        client.Login(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nError:\n" + ex.ToString());
                    }
                }
                try
                {
                    Console.WriteLine("{0} {1}", client.State, client.Ping());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError:\n" + ex.ToString());
                }
                if (random.Next(100) == 0)
                try
                {
                    client.Logout();
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError:\n" + ex.ToString());
                }
                finally
                {
                    client = null;
                }
                // Console.WriteLine("Hit any key...");
                // Console.ReadKey(true);
            }
        }
    }
}
