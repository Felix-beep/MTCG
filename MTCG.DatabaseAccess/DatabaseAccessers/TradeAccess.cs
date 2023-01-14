using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class TradeAccess
    {
        public static bool CreateTrade(string Username, string TradeID, string CardID, int Rating)
        {
            string text = "INSERT INTO \"Tradeoffer\" VALUES ( @u, @ci, @ti, @r)";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            command.Parameters.AddWithValue("ci", CardID);
            command.Parameters.AddWithValue("ti", TradeID);
            command.Parameters.AddWithValue("r", Rating);
            return DatabaseAccess.GetWriter(command);
        }

        public static TradeOffer GetTrade(string TradeId)
        {
            string text = "SELECT * FROM \"Tradeoffer\" WHERE \"TradeId\" = @ti ";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("ti", TradeId);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            reader.Read();
            if (!reader.HasRows) return null;

            string Name, CardId, Cardname;
            int Rating;
            try
            {
                Name = reader.GetString(0);
                CardId = reader.GetString(1);
                Rating = reader.GetInt16(3);
            } catch
            {
                Console.WriteLine("Error reading from Database.");
                reader.Close();
                return null;
            }
            reader.Close();

            return new TradeOffer(Name, CardId, TradeId, Rating);
        }

        public static List<TradeOffer> GetAllTrades()
        {
            string text = "SELECT * FROM \"Tradeoffer\"";
            var command = new NpgsqlCommand(text);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            List<TradeOffer> Offers = new();
            if (!reader.HasRows)
            {
                reader.Close();
                return Offers;
            }
            while (reader.Read() )
            {
                string Name, CardId, Cardname, TradeId;
                int Rating;

                try
                {
                    Name = reader.GetString(0);
                    CardId = reader.GetString(1);
                    TradeId = reader.GetString(2);
                    Rating = reader.GetInt16(3);
                } catch
                {
                    Console.WriteLine("Error when reading from Database.");
                    reader.Close();
                    return null;
                }

                Offers.Add(new TradeOffer(Name, CardId, TradeId, Rating));
            }
            reader.Close();

            return Offers;
        }

        public static bool DeleteAllTradesWithID(string Username, string TradeId)
        {
            string text = "DELETE FROM \"Tradeoffer\" where \"TradeId\" = @ti AND \"Username\" = @u;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("ti", TradeId);
            command.Parameters.AddWithValue("u", Username);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool DeleteAllTrades()
        {
            string text = "DELETE FROM \"Tradeoffer\" CASCADE;";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
