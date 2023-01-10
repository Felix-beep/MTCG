using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class PackAccess
    {
        public static bool CreatePack(string Username, List<string> CardNames)
        {
            if (CardNames.Count == 0) return false;
            string text = "INSERT INTO \"Pack\" VALUES";
            for (int i = 1; i <= CardNames.Count; i++)
            {
                text += $" (@u{i}, @cn{i})";
                if (i != CardNames.Count)
                {
                    text += ",";
                }
            }
            var command = new NpgsqlCommand(text);
            for (int i = 1; i <= CardNames.Count; i++)
            {
                command.Parameters.AddWithValue($"u{i}", Username);
                command.Parameters.AddWithValue($"cn{i}", CardNames[i - 1]);
            }
            return DatabaseAccess.GetWriter(command);
        }

        public static List<CardInstance> PopPack()
        {
            string text = "SELECT ct.\"Cardname\", ct.\"Power\", ct.\"Type\", ct.\"Element\", ct.\"Faction\", p.\"PackID\" ";
            text +=         "FROM \"Pack\" p ";
            text +=         "INNER JOIN \"CardTemplate\" ct ";
            text +=         "ON p.\"Cardname\" = ct.\"Cardname\"";
            text +=         "ORDER BY p.\"PackID\" DESC";
            text +=         "LIMIT 4";


            var command = new NpgsqlCommand(text);
            var reader = DatabaseAccess.GetReader(command);
            List<CardInstance> Cards = new();

            if (reader == null) return Cards;

            while (reader.Read())
            {
                string Name = reader.GetString(0);
                int Power = reader.GetInt32(1);
                string Type = reader.GetString(2);
                string Element = reader.GetString(3);
                string Faction = reader.GetString(4);

                CardTemplate BaseCard = new(Name, Power, Element, Type, Faction);
                CardInstance CardInstance = new(BaseCard);
                Cards.Add(CardInstance);
            }

            return Cards;
        }

        public static bool DeleteAllPacks()
        {
            string text = "DELETE FROM \"Pack\";";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
