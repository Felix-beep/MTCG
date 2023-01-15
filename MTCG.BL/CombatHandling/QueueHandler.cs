using MTCG.DatabaseAccess;
using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.MODELS;
using System.Numerics;
using System.Text.Json.Nodes;
using System.Threading;

namespace MTCG.BL.CombatHandling
{
    public static class QueueHandler
    {

        public static CurlResponse EnterQueue(string Username)
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

            if (QueueSpot.Open)
            {
                while(QueueSpot.Finished == false)
                {
                    Thread.Sleep(100);
                }
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

                QueueSpot.Finished = true;

                if (QueueSpot.Winner != 0)
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

                    if (!MoveElo(Winner, Loser))
                    {
                        response.Status = 409;
                        response.Success = false;
                        response.Message = "Unknown Database Error.";
                        return response;
                    }
                }
            }

            if (QueueSpot == null || !QueueSpot.Finished)
            {
                response.Status = 415;
                response.Success = false;
                response.Message = "Internal Combat Error";
                return response;
            }

            JsonObject Json = new()
            {
                {"Fight Result", (Won ? "You Won" : "You Lost") },
                {"Winner", (QueueSpot.Winner != 0 ? Winner.Name : "Draw") },
                {"Loser", Loser.Name},
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;

            return response;
        }

        public static bool MoveElo(User Winner, User Loser)
        {
            Stats StatsWinner = StatsAccess.GetStats(Winner.Name);
            Stats StatsLoser = StatsAccess.GetStats(Loser.Name);
            if(StatsWinner == null || StatsLoser == null) { return false; }

            Vector2 LowWins = new Vector2(-500, 50);
            Vector2 HighWins = new Vector2(500, 5);

            int EloDifference = StatsWinner.Elo - StatsLoser.Elo;
            int MaxEloDifference = 500;
            if (EloDifference > MaxEloDifference) EloDifference = MaxEloDifference;
            if (EloDifference < -MaxEloDifference) EloDifference = -MaxEloDifference;
            EloDifference += 500;
            float Percent = EloDifference / 1000;

            Vector2 Interpolated = Vector2.Lerp(LowWins, HighWins, Percent);
            
            StatsWinner.AddWin();
            StatsLoser.AddLoss();

            int EloToAdd = (int) Math.Round(Interpolated.Y);

            // if Elo To Add = 30, and Resulting Elo for Winner would be 1005, then subtract (1005-1000) from the Elo either player can gain
            int ResultingElo = StatsWinner.Elo + EloToAdd;
            if (ResultingElo > 1000) EloToAdd -= (ResultingElo - 1000);

            // if Elo To Add = 25, and Resulting Elo for Loser would be -5, then add (-5) to the Elo either player can gain
            ResultingElo = StatsLoser.Elo - EloToAdd;
            if (ResultingElo < 0) EloToAdd += ResultingElo;

            Console.WriteLine("- Moving Elo: " + EloToAdd);

            StatsWinner.AddElo(+EloToAdd);
            StatsLoser.AddElo(-EloToAdd);

            if(!StatsAccess.UpdateStats(StatsWinner)) return false;
            if(!StatsAccess.UpdateStats(StatsLoser)) return false;

            return true;
        }
    }
}
