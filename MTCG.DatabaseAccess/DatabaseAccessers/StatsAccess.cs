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
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
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
                Console.WriteLine("Error reading from Database.");
                reader.Close();
                return null;
            }
            reader.Close();
            return new Stats(Name, Elo, Wins, Losses);
        }

        public static List<Stats> GetAllStats()
        {
            string text = "SELECT * FROM \"Stats\"";
            var command = new NpgsqlCommand(text);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            List<Stats> list = new();
            if (!reader.HasRows)
            {
                reader.Close();
                return list;
            }

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

        public static bool UpdateStats(Stats newStats)
        {
            string text = "UPDATE \"Stats\" SET \"Elo\" = @e, \"Wins\" = @w, \"Losses\" = @l WHERE \"Username\" = @u ;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("e", newStats.Elo);
            command.Parameters.AddWithValue("w", newStats.Wins);
            command.Parameters.AddWithValue("l", newStats.Losses);
            command.Parameters.AddWithValue("u", newStats.Username);
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
            string text = "DELETE FROM \"Stats\" CASCADE;";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
