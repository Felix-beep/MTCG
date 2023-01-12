using MTCG.Models;
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
        public static bool CreateTrade(string Username, string CardID, string Cardname, string TradeID, int Rating)
        {
            string text = "INSERT INTO \"CreateTrade\" VALUES ( @u, @ci, @cn, @ti, @r)";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            command.Parameters.AddWithValue("ci", CardID);
            command.Parameters.AddWithValue("cn", Cardname);
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
                Cardname = reader.GetString(2);
                Rating = reader.GetInt16(4);
            } catch
            {
                Console.WriteLine("Error reading from Database.");
                reader.Close();
                return null;
            }
            reader.Close();

            return new TradeOffer(Name, CardId, Cardname, TradeId, Rating);
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
                    Cardname = reader.GetString(2);
                    TradeId = reader.GetString(3);
                    Rating = reader.GetInt16(4);
                } catch
                {
                    Console.WriteLine("Error when reading from Database.");
                    reader.Close();
                    return null;
                }

                Offers.Add(new TradeOffer(Name, CardId, Cardname, TradeId, Rating));
            }
            reader.Close();

            return Offers;
        }

        public static bool DeleteAllTradesWithID(string TradeId)
        {
            string text = "DELETE FROM \"Tradeoffer\" where \"TradeId\" = @ti;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("ti", TradeId);
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
