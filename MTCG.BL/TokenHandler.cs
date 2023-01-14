using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.DatabaseAccess.DatabaseAccessers;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace MTCG.BL
{
    public static class TokenHandler
    {
        private static Dictionary<string, string> Tokens = new();
        public static string CreateToken(string Username)
        {
            string response = null;

            Guid uuid = Guid.NewGuid();

            Console.WriteLine($"Adding Token for {Username}");

            if(Tokens.ContainsKey(Username))
            {
                Tokens[Username] = uuid.ToString();
            } else
            {
                Tokens.Add(Username, uuid.ToString());
            }

            return uuid.ToString();
        }

        public static string AuthenticateUser(string Token)
        {
            string Username = Token.Replace("-mtcgToken", "");
            Console.WriteLine($"Looking for {Token}");
            if(Tokens.ContainsKey(Username)) { return Username; }

            if (Tokens.ContainsValue(Token)) {
                return Tokens.First(x => x.Value == Token).Key;
            }

            return null;
        }
    }
}
