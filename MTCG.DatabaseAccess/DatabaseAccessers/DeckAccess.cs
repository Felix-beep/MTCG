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
        public static Deck GetStack(string Username)
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

            if (reader == null) return null;

            Deck newDeck = new(Username);
            while (reader.Read())
            {
                int Rating = reader.GetInt32(1);
                string CardID = reader.GetString(2);
                string Name = reader.GetString(3);
                int Power = reader.GetInt32(4);
                string Type = reader.GetString(5);
                string Element = reader.GetString(6);
                string Faction = reader.GetString(7);

                CardTemplate BaseCard = new(Name, Power, Element, Type, Faction);
                CardInstance CardInstance = new(Rating, Name, CardID, BaseCard);
                newDeck.DeckList.Add(CardInstance);
            }

            return newDeck;
        }
    }
}
