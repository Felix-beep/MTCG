using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MTCG.BL
{
    public static class StackHandler
    {
        public static CurlResponse GetStack(string Username)
        {
            CurlResponse response = new();

            User Buyer = UserAccess.GetUser(Username);

            if (Buyer == null)
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "User does not exist.";
                return response;
            }

            Stack StackOut = StackAccess.GetStack(Username);

            if (StackOut == null || StackOut.CardList.Count == 0)
            {
                response.Status = 201;
                response.Success = true;
                response.Message = "This User doesn't own any Cards.";
                return response;
            }

            JsonArray CardObjects = new();

            foreach (CardInstance card in StackOut.CardList)
            {
                JsonObject JsonCard = new()
                {
                    { "Id", card.ID },
                    { "Name", card.CardName },
                    { "Rating", card.Rating },
                    { "Power", card.EffectivePower },
                };
                CardObjects.Add(JsonCard);
            }

            JsonObject Json = new()
            {
                { "Owned Cards", CardObjects }
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;

            return response;
        }
    }
}
