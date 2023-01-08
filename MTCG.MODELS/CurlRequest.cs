using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Newtonsoft;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace MTCG.MODELS
{
    public class CurlRequest
    {
        public string UnparsedMessage { get; private set; }

        private Dictionary<string, string> HeaderDictionary = new();
        public string? ContentType { get; private set; }
        public string? Token { get; private set; }


        public string? ContentLength { get; private set; }


        public string Method { get; private set; } // e.g. /User
        public string? MethodInfo { get; private set; } // e.g. {Method}/admin
        public string MethodType { get; private set; } // e.g. POST

        public string? Payload { get; private set; }
        public dynamic? DynamicJsonObject { get; set; }

        public CurlRequest(string data)
        {
            data = data.Replace("\r\n", "\n").Replace("\r", "\n");
            UnparsedMessage = data;
            
            List<string> lines = UnparsedMessage.Split("\n").ToList();
            string[] words = lines[0].Split(" ");
            MethodType = words[0];
            Method = words[1];

            CheckForMethodInfo();
            
            lines.RemoveAt(0);
            foreach(string line in lines)
            {
                if(line == "") break;

                words = line.Split(": ");
                if (words.Length == 1)
                {
                    HeaderDictionary.Add(words[0], "");
                } else if (words.Length == 2)
                {
                    HeaderDictionary.Add(words[0], words[1]);
                }
            }

            if (HeaderDictionary.ContainsKey("Authorization")){
                HeaderDictionary["Authorization"] = HeaderDictionary["Authorization"].Replace("Bearer ", "");
            }

            Token = HeaderDictionary.ContainsKey("Authorization") ? HeaderDictionary["Authorization"] : null;
            ContentType = HeaderDictionary.ContainsKey("Content-Type") ? HeaderDictionary["Content-Type"] : null;
            ContentLength = HeaderDictionary.ContainsKey("Content-Length") ? HeaderDictionary["Content-Length"] : null; 
            
            if (ContentLength != null && ContentLength != "0" && ContentLength != "")
            {
                Payload = lines[lines.Count - 1];
            }
            
            if(ContentType == "application/json" && Payload != null)
            {
                DynamicJsonObject = JsonConvert.DeserializeObject<dynamic>(Payload);
            }
        }

        private void CheckForMethodInfo()
        {
            string[] parts = Method.Split("/");
            Method = parts[1];
            if (parts.Length > 2)
            {
                MethodInfo = parts[2];
            }
        }

        public void Print()
        {
            Console.WriteLine("MethodName: " + MethodType);
            Console.WriteLine("Method: " + Method);
            Console.WriteLine("MethodInfo: " + MethodInfo);
            foreach (var item in HeaderDictionary)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Payload: " + Payload);
            if (DynamicJsonObject != null)
            {
                foreach (var obj in DynamicJsonObject)
                {
                    Console.WriteLine(obj);
                }
            }
        }
    }
}
