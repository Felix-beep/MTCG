using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class DeckAccess
    {
        public static bool CreateDeck(string Username, List<string> Cards)
        {
            if (Cards.Count == 0) return false;
            string text = "INSERT INTO \"Deck\" VALUES";
            for (int i = 1; i <= Cards.Count; i++)
            {
                text += $" (@u{i}, @p{i})";
                if (i != Cards.Count)
                {
                    text += ",";
                }
            }
            var command = new NpgsqlCommand(text);
            for (int i = 1; i <= Cards.Count; i++)
            {
                command.Parameters.AddWithValue($"u{i}", Username);
                command.Parameters.AddWithValue($"p{i}", Cards[i - 1]);
            }
            return DatabaseAccess.GetWriter(command);
        }
        public static Deck GetDeck(string Username)
        {
            string text = "SELECT \"Deck\".\"Username\", ";
            text +=         "\"CardInstance\".\"Rating\", \"CardInstance\".\"CardID\", ";
            text +=         "\"CardTemplate\".\"Cardname\", \"CardTemplate\".\"Power\", \"CardTemplate\".\"Type\",  \"CardTemplate\".\"Faction\"";
            text +=         "INNER JOIN \"CardInstance\" ON \"CardInstance\".\"CardID\" = \"Deck\".\"CardID\" ";
            text +=         "INNER JOIN \"CardTemplate\" ON \"CardTemplate\".\"Cardname\" = \"CardInstance\".\"Cardname\" ";
            text +=         "WHERE \"Deck\".\"Username\" = @u";
            text +=         "ORDER BY \"Deck\".\"ID\"";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            var reader = DatabaseAccess.GetReader(command);
            Deck newDeck = new(Username);

            if (reader == null) return newDeck;
            if (!reader.HasRows) return newDeck;
            while (reader.Read())
            {
                int Rating, Power;
                string CardID, Name, Type, Element, Faction;
                try
                {
                    Rating = reader.GetInt32(1);
                    CardID = reader.GetString(2);
                    Name = reader.GetString(3);
                    Power = reader.GetInt32(4);
                    Type = reader.GetString(5);
                    Element = reader.GetString(6);
                    Faction = reader.GetString(7);
                } catch
                {
                    Console.WriteLine("Error reading from Database");
                    reader.Close();
                    return null;
                }

                CardTemplate BaseCard = new(Name, Power, Element, Type, Faction);
                CardInstance CardInstance = new(Rating, Name, CardID, BaseCard);
                newDeck.DeckList.Add(CardInstance);
            }
            reader.Close();

            return newDeck;
        }

        public static bool DeleteFromDeck(string Username, string cardId)
        {
            string text = "DELETE FROM \"Deck\" WHERE \"USERNAME\" = @u AND \"CardID\" = @ci";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue($"u", Username);
            command.Parameters.AddWithValue($"ci", cardId);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool DeleteDeck(string Username)
        {
            string text = "DELETE FROM \"Deck\" WHERE \"USERNAME\" = @u";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue($"u", Username);
            return DatabaseAccess.GetWriter(command);
        }
        
        public static bool AddToDeck(string Username, List<string> Cards)
        {
            return CreateDeck(Username, Cards);
        }

        public static bool DeleteAllStacks()
        {
            string text = "DELETE FROM \"Deck\";";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
