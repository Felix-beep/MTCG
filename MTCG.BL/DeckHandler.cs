using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.MODELS;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MTCG.BL
{
    public static class DeckHandler
    {
        public static CurlResponse CreateDeck(string Username, List<string> CardIds)
        {
            CurlResponse response = new();

            User Buyer = UserAccess.GetUser(Username);

            if (Buyer == null)
            {
                response.Status = 403;
                response.Success = false;
                response.Message = "User does not exist.";
                return response;
            }

            Deck DeckOut = DeckAccess.GetDeck(Username);

            if (DeckOut != null || DeckOut.DeckList.Count != 0)
            {
                if (!DeckAccess.DeleteDeck(Username))
                {
                    response.Status = 409;
                    response.Success = false;
                    response.Message = "Unknown Database Error.";
                    return response;
                }
            }

            if(!DeckAccess.CreateDeck(Username, CardIds))
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Deck could not be created.";
                return response;
            }

            response.Status = 200;
            response.Success = true;
            response.Message = "The deck has been successfully configured.";
            return response;
        }

        public static CurlResponse GetDeck(string Username)
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

            Deck DeckOut = DeckAccess.GetDeck(Username);

            if (DeckOut == null || DeckOut.DeckList.Count == 0)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "This User doesn't have a Deck.";
                return response;
            }

            bool ValidDeck = true;
            JsonArray CardObjects = new();

            foreach (CardInstance card in DeckOut.DeckList)
            {
                bool ValidCard = ValidateCard(Username, card.ID);
                if (!ValidCard) ValidDeck = false;
                string Owner = ValidCard ? "Owned" : "Not Owned";

                JsonObject JsonCard = new()
                {
                    { "Id", card.ID },
                    { "Ownership", Owner },
                    { "Name", card.CardName },
                    { "Rating", card.Rating },
                    { "Power", card.EffectivePower },
                };
                CardObjects.Add(JsonCard);
            }

            JsonObject Json = new()
            {
                { "Valid Deck", ValidDeck ? "True" : "false" },
                { "Owned Cards", CardObjects },
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;

            return response;
        }

        public static bool ValidateCard(string Username, string CardID)
        {
            return StackAccess.FindCardInStack(Username, CardID);
        }

        public static bool ValidateDeck(string Username)
        {
            Deck myDeck = DeckAccess.GetDeck(Username);
            if (myDeck == null || myDeck.DeckList.Count != 4)
            {
                return false;
            }

            bool Error = false;

            foreach(CardInstance Card in myDeck.DeckList)
            {
                if (!ValidateCard(Username, Card.ID)) Error = true;
            }

            return Error;
        }
    }
}
