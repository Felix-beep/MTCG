using MTCG.DatabaseAccess;
using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.MODELS;
using System.Text.Json.Nodes;

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

            QueueEntry QueueSpot = QueueAccess.EnterQueue(user, deck);

            bool Won = false;
            User Winner = null;
            User Loser = null;

            if (QueueSpot.Open) {
                QueueSpot.MutexFinished.WaitOne();
                QueueSpot.MutexFinished.ReleaseMutex();
                if (QueueSpot.Winner == 1) Won = true;
                if (Won)
                {
                    Winner = QueueSpot.User1;
                    Loser = QueueSpot.User2;
                }
                else
                {
                    Winner = QueueSpot.User2;
                    Loser = QueueSpot.User1;
                }
            } else
            {
                CombatHandler myCombat = new CombatHandler(QueueSpot);
                int intWinner = myCombat.StartCombat();
                switch (intWinner)
                {
                    case 0: 
                    case 1: 
                    case 2: QueueSpot.Winner = intWinner; break;
                    default:
                        response.Status = 409;
                        response.Success = false;
                        response.Message = "Unknown Error during Combat.";
                        return response;
                }

                if(QueueSpot.Winner != 0)
                {
                    if (QueueSpot.Winner == 2) Won = true;

                    if (Won)
                    {
                        Winner = QueueSpot.User2;
                        Loser = QueueSpot.User1;
                    } else
                    {
                        Winner = QueueSpot.User1;
                        Loser = QueueSpot.User2;
                    }

                    if (MoveElo(Winner, Loser))
                    {
                        response.Status = 409;
                        response.Success = false;
                        response.Message = "Unknown Database Error.";
                        return response;
                    }
                }
                QueueSpot.MutexFinished.ReleaseMutex();
            }

            JsonObject Json = new()
            {
                {"Fight Result", Won.ToString()},
                {"Winner", Winner.Name },
                {"Loser", Loser.Name},
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;

            return response;
        }

        private static bool MoveElo(User Winner, User Loser)
        {
            Stats StatsWinner = StatsAccess.GetStats(Winner.Name);
            Stats StatsLoser = StatsAccess.GetStats(Loser.Name);
            if(StatsWinner == null || StatsLoser == null) { return false; }

            StatsWinner.AddWin();
            StatsLoser.AddLoss();

            int EloDifference = Math.Abs(StatsWinner.Elo - StatsLoser.Elo);
            int MaxAccountedDifference = 1000;
            float Percent = EloDifference / MaxAccountedDifference;
            if(Percent > 1) { Percent = 1; }

            float EloperWin = 100;

            float AchievedElo = EloperWin * Percent;
            int EloToAdd = (int) Math.Round((double)AchievedElo);

            StatsWinner.AddElo(+EloToAdd);
            StatsLoser.AddElo(-EloToAdd);

            if(!StatsAccess.UpdateStats(StatsWinner)) return false;
            if(!StatsAccess.UpdateStats(StatsLoser)) return false;

            return true;
        }
    }
}
