using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.TCPServer
{
    public class MyServer
    {
        private static readonly List<Thread> threads = new();
        public static void Main()
        {
            TcpListener? server = null;
            try
            {
                // Set the TcpListener on port 10001.
                Int32 port = 10001;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    MyTcpHandler handler = new(client);
                    Thread newthread = new(new ThreadStart(handler.HandleRequest));
                    threads.Add(newthread);
                    newthread.Start();
                    threads.Remove(newthread);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                server?.Stop();
            }
            Console.WriteLine("\nWaiting for Threads to stop...");
            // implement thread waiting
            Console.WriteLine("\nThreads were stopped");

            Console.WriteLine("\nClosing Program!");
        }
    }
}
