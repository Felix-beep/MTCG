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

        public static Stats GetLeaderbaord(string Username)
        {
            string text = "SELECT * FROM \"Leaderboard\" ORDER BY \"Elo\" DESC";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            reader.Read();
            string Name = reader.GetString(0);
            int Elo = reader.GetInt32(1);
            int Wins = reader.GetInt32(2);
            int Losses = reader.GetInt32(3);
            return new Stats(Name, Elo, Wins, Losses);
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
            string text = "DELETE FROM \"Leaderboard\";";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
