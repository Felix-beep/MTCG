using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.MODELS;

namespace MTCG.BL.CombatHandling
{
    public static class QueueHandler
    {
        public static CurlResponse EnterQueue(string Username, string DeckID)
        {
            CurlResponse response = new();
            
            User user = UserAccess.GetUser(Username);
            if(user == null )
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "User doesnt exist.";
                return response;
            }

            Deck deck = DeckAccess.GetDeck(Username);
            if (deck == null)
            {
                response.Status = 404;
                response.Success = false;
                response.Message = "Deck doesnt exist.";
                return response;
            }

            if (DeckHandler.ValidateDeck(Username))
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Deck is not valid.";
                return response;
            }



            return response;
        }
    }
}
