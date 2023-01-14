using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTCG.BL
{
    public static class TradeHandler
    {
        public static CurlResponse CreateTrade(string Username, string Id, string CardId, int Rating)
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

            if(!StackAccess.FindCardInStack(Username, CardId))
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "The User does not own that card.";
                return response;
            }

            if(!TradeAccess.CreateTrade(Username, Id, CardId, Rating))
            {
                response.Status = 415;
                response.Success = false;
                response.Message = "Unknown Database Error.";
                return response;
            }

            response.Status = 200;
            response.Success = true;
            response.Message = "Tradedeal sucessfully created.";
            return response;
        }

        public static CurlResponse BuyTrade(string Username, string TradeId, string CardId)
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

            TradeOffer Trade = TradeAccess.GetTrade(TradeId);
            if(Trade == null)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "The provided deal ID was not found.";
                return response;
            }

            CardInstance CardToGet = CardInstaceAccess.GetCardInstance(Trade.CardId);

            CardInstance CardToGive = CardInstaceAccess.GetCardInstance(CardId);

            if(CardToGive.Rating <= Trade.Rating)
            {
                response.Status = 403;
                response.Success = false;
                response.Message = "The requirements are not met(Type, MinimumDamage).";
                return response;
            }

            if(!MoveFromUserToUser(Username, CardToGive, Trade.Username))
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Unknown Database Error when giving card.";
                return response;
            }

            if (!MoveFromUserToUser(Trade.Username, CardToGet, Username))
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Unknown Database Error when receiving card.";
                return response;
            }

            response.Status = 200;
            response.Success = true;
            response.Message = "Trading deal successfully executed.";
            return response;
        }

        public static CurlResponse DeleteTrade(string Username, string TradeId)
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

            if (!TradeAccess.DeleteAllTradesWithID(Username, TradeId))
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "The provided deal ID was not found.";
                return response;
            }

            response.Status = 200;
            response.Success = true;
            response.Message = "Trading deal succesfully deleted.";
            return response;
        }

        public static CurlResponse GetTradeDeals()
        {
            CurlResponse response = new();

            List<TradeOffer> Trades = TradeAccess.GetAllTrades();

            if(Trades == null)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "No TradeOffers could be found.";
                return response;
            }

            JsonArray CardObjects = new();

            foreach (TradeOffer trade in Trades)
            {
                JsonObject JsonCard = new()
                {
                    { "Id", trade.TradeId },
                    { "Ownership", trade.Username },
                    { "CardToTrade", trade.CardId },
                    { "Minimum Rating required", trade.Rating },
                };
                CardObjects.Add(JsonCard);
            }
            
            JsonObject Json = new()
            {
                { "TradeOffers", CardObjects },
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;
            return response;
        }

        private static bool MoveFromUserToUser(string Giver, CardInstance CardToGive, string Receiver)
        {
            if (!StackAccess.FindCardInStack(Giver, CardToGive.ID))
            {
                return false;
            }

            if (!StackAccess.DeleteFromStack(Giver, CardToGive.ID))
            {
                return false;
            }

            List<string> CardsToGive = new();
            CardsToGive.Add(CardToGive.ID);

            if (!StackAccess.AddToStack(Receiver, CardsToGive))
            {
                StackAccess.AddToStack(Giver, CardsToGive);
                return false;
            }

            return true;
        }
    }
}
