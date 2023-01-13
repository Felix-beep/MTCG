using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Text.Json.Nodes;

namespace MTCG.BL
{
    public static class PackHandler
    {
        public static CurlResponse CreatePackage(string Username, List<Tuple<string, string>> CardIds)
        {
            CurlResponse response = new();

            foreach(var Pair in CardIds)
            {
                if (CardInstaceAccess.GetCardInstance(Pair.Item2) != null)
                {
                    response.Status = 409;
                    response.Success = false;
                    response.Message = "At least one card in the packages already exists.";
                    return response;
                }
            }

            List<string> validCardIds = new();

            foreach (var Pair in CardIds)
            {
                if (!CardInstaceAccess.CreateCardInstance(CardInstance.GetRandomRating(), Pair.Item1, Pair.Item2))
                {
                    response.Status = 409;
                    response.Success = false;
                    response.Message = "Unknown Database Error.";
                    return response;
                }

                validCardIds.Add(Pair.Item2);
            }

            if (!PackAccess.CreatePack(Username, validCardIds))
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Unknown Database Error.";
                return response;
            }

            response.Status = 201;
            response.Success = true;
            response.Message = "Package and cards successfully created.";
            return response;
        }

        public static CurlResponse BuyPackage(string Username)
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

            if(Buyer.Gold < 5)
            {
                response.Status = 403;
                response.Success = false;
                response.Message = "User does not have enough money for buying a card package.";
                return response;
            }

            List<CardInstance> Cards = PackAccess.PopPack();

            if(Cards == null)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "No card package available for buying.";
                return response;
            }

            JsonArray CardObjects = new();
            
            foreach(CardInstance card in Cards)
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
                { "Received Cards", CardObjects }
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;
            return response;
        }
    }
}
