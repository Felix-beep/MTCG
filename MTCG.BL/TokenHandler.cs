using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.Models;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace MTCG.BL
{
    public static class TokenHandler
    {
        private static Dictionary<string, string> Tokens = new();
        public static bool CreateToken(string Username)
        {
            bool response = true;

            Guid uuid = Guid.NewGuid();

            if(Tokens.ContainsKey(Username))
            {
                Tokens[Username] = uuid.ToString();
            } else
            {
                Tokens.Add(Username, uuid.ToString());
            }

            return response;
        }

        public static string AuthenticateUser(string Token)
        {
            string Username = Token.Replace("-mtcgToken", "");
            if(Tokens.ContainsKey(Username)) { return Username; }

            if (Tokens.ContainsValue(Token)) {
                return Tokens.First(x => x.Value == Token).Key;
            }

            return null;
        }
    }
}
