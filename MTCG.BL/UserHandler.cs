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
    public static class UserHandler
    {
        public static CurlResponse CreateUser(string Username, string Password)
        {
            CurlResponse response = new();

            if(Username == null || Username.Length < 6 || Password == null || Password.Length < 6) {
                response.Status = 409;
                response.Success = false;
                response.Message = "Username and Password have to be at least 6 Characters long.";
                return response;
            }

            if (UserAccess.GetUser(Username) != null)
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "User with same username already registered.";
                return response;
            }

            if (!UserAccess.CreateUser(Username, Password))
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Unknown Database Error.";
                return response;
            }

            response.Status = 201;
            response.Success = true;
            response.Message = "User succesfully created.";
            return response;
        }

        public static CurlResponse GetUser(string Username)
        {
            CurlResponse response = new();

            User UserOut = UserAccess.GetUser(Username);

            if (UserOut == null)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            JsonObject Json = new()
            {
                {"Name", UserOut.Name},
                {"Bio", UserOut.Bio },
                {"Image", UserOut.Image},
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;

            return response;
        }

        public static CurlResponse LoginUser(string Username, string Password)
        {
            CurlResponse response = new();

            User UserOut = UserAccess.GetUser(Username);

            if (UserOut == null)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "Invalid username/password provided.";
                return response;
            }

            if(UserOut.Password != Password)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "Invalid username/password provided.";
                return response;
            }

            // create Token

            string Token = $"{Username}-mtcgToken";

            JsonObject Json = new()
            {
                {"Token", Token},
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;

            return response;
        }

        public static CurlResponse UpdateUser(string Username, string Name, string Bio, string Image)
        {
            CurlResponse response = new();

            User UserOut = UserAccess.GetUser(Username);

            if(UserOut == null)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            if(UserAccess.EditUser(Name, Bio, Image))
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Unknown Database Error.";
                return response;
            }

            response.Status = 200;
            response.Success = true;
            response.Message = "user sucessfully updated.";
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
