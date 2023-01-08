using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    public static class UserHandler
    {
        public static CurlResponse CreateUser(string Username, string Password)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse GetUser(string Username)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse LoginUser(string Username, string Password)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse UpdateUser(string Username, string Name, string Bio, string Image)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse AuthenticateUser(string Username)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse ValidateUser(string Username)
        {
            CurlResponse response = new();

            return response;
        }
    }
}
