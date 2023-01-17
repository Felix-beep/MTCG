using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using MTCG.MODELS;
using static System.Net.WebRequestMethods;

namespace MTCG.TCPServer
{
    public class MyTcpHandler
    {
        private TcpClient Client;
        public MyTcpHandler(TcpClient client)
        {
            Client = client;
        }

        ~MyTcpHandler()
        {
            Console.WriteLine("Disposing of Client");
            Client.Dispose();
        }

        public void HandleRequest()
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String? data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = Client.GetStream();

            int i;

            // Loop to receive all the data sent by the client.
            data = "";
            while (stream.DataAvailable || (data == ""))
            {                                                               // read and decode stream
                i = stream.Read(bytes, 0, bytes.Length);
                data += Encoding.ASCII.GetString(bytes, 0, i);
            }

            CurlRequest myHttpRequest = new CurlRequest(data);

            Console.WriteLine("\n------------Request:--------------");
            Console.WriteLine(data);
            Console.WriteLine("\n------------HTTp:   --------------");
            myHttpRequest.Print();

            RequestHandler handle = new(myHttpRequest);
            handle.HandleRequest();

            string reply = FormReply(handle.Response);

            Console.WriteLine("------------Reply:  --------------\n" + reply);

            byte[] dbuf = Encoding.ASCII.GetBytes(reply);
            stream.Write(dbuf, 0, dbuf.Length);                    // send a response

        }

        private static string FormReply(CurlResponse response)
        {
            string reply = "";
            reply += "HTTP/1.1 ";
            reply += response.Status;
            reply += response.Success ? " OK" : " ERROR";
            reply += "\n";

            string content = response.Message + "\n";
            reply += fillCurlResponse(!response.Json ? content : response.parseDictionaryToString());
            return reply;
        }
        private static string fillCurlResponse(string? content)
        {
            string reply = "";
            //reply += "Content-Type: text/plain\n\n";
            if (string.IsNullOrEmpty(content))
            {                                                                   
                reply += "Content-Length: 0\n";
            }
            else
            {
                reply += $"Content-Length: {content.Length}\n";
            }
            

            if (!string.IsNullOrEmpty(content)) { reply += "\n" + content; }

            return reply;
        }
    }
}