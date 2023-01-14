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
        public static CurlResponse CreatePackage(string Username, List<Tuple<string, string>> CardNamesAndIds)
        {
            CurlResponse response = new();

            foreach(var Pair in CardNamesAndIds)
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
            List<string> validCardNames = new();

            foreach (var Pair in CardNamesAndIds)
            {
                if (!CardInstaceAccess.CreateCardInstance(CardInstance.GetRandomRating(), Pair.Item1, Pair.Item2))
                {
                    response.Status = 409;
                    response.Success = false;
                    response.Message = "Unknown Database Error.";
                    return response;
                }

                validCardIds.Add(Pair.Item1);
                validCardNames.Add(Pair.Item2);
            }

            if (!PackAccess.CreatePack(Username, validCardNames, validCardIds))
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

            Tuple<string, List<CardInstance>> Cards = PackAccess.GetPack();

            if(Cards == null)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "No card package available for buying.";
                return response;
            }

            if (!UserAccess.ChangeUserGold(Buyer.Name, Buyer.Gold - 5))
            {
                response.Status = 403;
                response.Success = false;
                response.Message = "Unknown Database Error.";
                return response;
            }

            PackAccess.DeleteAllPacksWithID(Cards.Item1);

            List<string> CardIds = new();
            foreach(var inst in Cards.Item2)
            {
                CardIds.Add(inst.ID);
            }

            StackAccess.AddToStack(Buyer.Name, CardIds);

            JsonArray CardObjects = new();
            
            foreach(CardInstance card in Cards.Item2)
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
