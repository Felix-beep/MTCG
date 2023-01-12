using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class StatsAccess
    {
        public static bool CreateStats(string Username)
        {
            string text = "INSERT INTO \"Stats\" VALUES ( @u )";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            return DatabaseAccess.GetWriter(command);
        }

        public static Stats GetStats(string Username)
        {
            string text = "SELECT * FROM \"Stats\" WHERE \"Username\" = @u ";
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

        public static List<Stats> GetAllStats()
        {
            string text = "SELECT * FROM \"Stats\"";
            var command = new NpgsqlCommand(text);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            List<Stats> list = new();
            if (reader.HasRows) return list;

            while (reader.Read())
            {
                string Name;
                int Elo, Wins, Losses;

                try
                {
                    Name = reader.GetString(0);
                    Elo = reader.GetInt32(1);
                    Wins = reader.GetInt32(2);
                    Losses = reader.GetInt32(3);
                } catch
                {
                    Console.WriteLine("Error when reading from Database.");
                    reader.Close();
                    return null;
                }

                list.Add(new Stats(Name, Elo, Wins, Losses));
            }

            reader.Close();
            return list;
        }

        public static bool UpdateStats(string Username, int Elo, int Wins, int Losses)
        {
            string text = "UPDATE \"Stats\" SET \"Elo\" = @e, \"Wins\" = @w, \"Losses\" = @l WHERE \"Username\" = @u ;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("e", Elo);
            command.Parameters.AddWithValue("w", Wins);
            command.Parameters.AddWithValue("l", Losses);
            command.Parameters.AddWithValue("u", Username);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool DeleteStats(string Username)
        {
            string text = "DELETE FROM \"Stats\" where \"Username\" = @u;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool DeleteAllStats()
        {
            string text = "DELETE FROM \"Stats\";";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
