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
    public static class StatsHandler
    {
        public static CurlResponse GetStats(string Username)
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

            Stats StatsOut = StatsAccess.GetStats(Username);

            if (StatsOut == null)
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Unknown Database Error.";
                return response;
            }

            JsonObject Json = new()
            {
                { "Name", StatsOut.Username},
                { "Elo", StatsOut.Elo},
                { "Wins", StatsOut.Wins},
                { "Losses", StatsOut.Losses},
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;
            return response;
        }

        public static CurlResponse GetLeaderboard()
        {
            CurlResponse response = new();

            List<Tuple<int, string, int>> Leaderboard = LeaderboardAccess.GetLeaderboard();

            if(Leaderboard == null)
            {
                response.Status = 409;
                response.Success = false;
                response.Message = "Unknown Database Error.";
                return response;
            }

            JsonArray JsonArray = new();

            foreach(var Ranking in Leaderboard)
            {
                JsonObject Place = new()
                {
                    { "Rank", Ranking.Item1},
                    { "Player", Ranking.Item2},
                    { "Elo", Ranking.Item3},
                };

                JsonArray.Add(Place);
            }

            JsonObject Json = new()
            {
                { "Ranking", JsonArray },
            };

            response.Status = 200;
            response.Success = true;
            response.Json = true;
            response.JsonList = Json;
            return response;
        }
    }
}
