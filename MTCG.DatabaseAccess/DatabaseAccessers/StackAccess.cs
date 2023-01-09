using MTCG.Models;
using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class StackAccess
    {
        public static bool CreateStack(string Username, List<string> Cards)
        {
            if(Cards.Count == 0) return false;
            string text = "INSERT INTO \"Stack\" VALUES";
            for (int i = 1; i <= Cards.Count; i++)
            {
                text += $" (@u{i}, @p{i})";
                if(i != Cards.Count)
                {
                    text += ",";
                }
            }
            var command = new NpgsqlCommand(text);
            for (int i = 1; i <= Cards.Count; i++)
            {
                command.Parameters.AddWithValue($"u{i}", Username);
                command.Parameters.AddWithValue($"p{i}", Cards[i-1]);
            }
            return DatabaseAccess.GetWriter(command);
        }

        public static Stack GetStack(string Username)
        {
            string text = "SELECT \"Stack\".\"Username\", ";
            text +=         "\"CardInstance\".\"Rating\", \"CardInstance\".\"CardID\", ";
            text +=         "\"CardTemplate\".\"Cardname\", \"CardTemplate\".\"Power\", \"CardTemplate\".\"Type\",  \"CardTemplate\".\"Faction\"";
            text +=         "INNER JOIN \"CardInstance\" ON \"CardInstance\".\"CardID\" = \"Stack\".\"CardId\" ";
            text +=         "INNER JOIN \"CardTemplate\" ON \"CardTemplate\".\"Cardname\" = \"CardInstance\".\"Cardname\" ";
            text +=         "WHERE \"Stack\".\"Username\" = @u";
            text +=         "ORDER BY \"Stack\".\"Id \"";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;

            Stack Stack = new(Username);
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
                Stack.CardList.Add(CardInstance);
            }

            return Stack;
        }

        public static bool EditUser(string Username, string Bio, string Picture)
        {
            string text = "UPDATE \"User\" SET \"Bio\" = @b, \"Picture\" = @p WHERE \"Username\" = @u ;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("@b", Bio);
            command.Parameters.AddWithValue("@p", Picture);
            command.Parameters.AddWithValue("@u", Username);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool DeleteAllUsers()
        {
            string text = "DELETE FROM \"User\";";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
