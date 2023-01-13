using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class LeaderboardAccess
    {
        public static bool AddUserToLeaderboard(string Username, int Elo)
        {
            string text = "INSERT INTO \"Leaderboard\" VALUES ( @u, @e )";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            command.Parameters.AddWithValue("e", Elo);
            return DatabaseAccess.GetWriter(command);
        }

        public static List<Tuple<int, string, int>> GetLeaderboard()
        {
            string text = "SELECT * FROM \"Leaderboard\" ORDER BY \"Elo\" DESC";
            var command = new NpgsqlCommand(text);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;

            List<Tuple<int, string, int>> Ranking = new();
            if (!reader.HasRows)
            {
                reader.Close();
                return Ranking;
            }
            int Placement = 0;
            while (reader.Read() != null)
            {
                Placement++;
                int Place = Placement;
                int Elo;
                string Name;
                try
                {
                    Name = reader.GetString(0);
                    Elo = reader.GetInt32(1);
                } catch
                {
                    reader.Close();
                    Console.WriteLine("Error reading from Database.");
                    return null;
                }
                
                Ranking.Add(new(Place, Name, Elo));
            }
            reader.Close();
            return Ranking;
        }

        public static bool UpdateLeaderboard(string Username, int Elo)
        {
            string text = "UPDATE \"Leaderboard\" SET \"Elo\" = @e WHERE \"Username\" = @u;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("e", Elo);
            command.Parameters.AddWithValue("u", Username);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool DeleteLeaderboard()
        {
            string text = "DELETE FROM \"Leaderboard\" CASCADE;";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
