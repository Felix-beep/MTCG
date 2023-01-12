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
            text +=         "ON p.\"Cardname\" = ct.\"Cardname\" ";
            text +=         "ORDER BY p.\"PackID\" DESC ";
            text +=         "LIMIT 4";


            var command = new NpgsqlCommand(text);
            var reader = DatabaseAccess.GetReader(command);
            List<CardInstance> Cards = new();
            string PackID = "";

            if (reader == null) return null;
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            while (reader.Read())
            {
                string Name, Type, Element, Faction;
                int Power;
                try
                {
                    Name = reader.GetString(0);
                    Power = reader.GetInt32(1);
                    Type = reader.GetString(2);
                    Element = reader.GetString(3);
                    Faction = reader.GetString(4);
                } catch
                {
                    Console.WriteLine("Error reading from Database.");
                    reader.Close();
                    return null;
                }

                // get first PackID that was found
                if(PackID == "") PackID = reader.GetString(5);

                CardTemplate BaseCard = new(Name, Power, Element, Type, Faction);
                CardInstance CardInstance = new(BaseCard);
                Cards.Add(CardInstance);
            }
            reader.Close();

            if(!DeleteAllPacksWithID(PackID))
            {
                Console.WriteLine("Error when deleting pack by ID!");
                return new();
            }

            if(Cards.Count != 4)
            {
                return PopPack();
            }

            return Cards;
        }
        public static bool DeleteAllPacksWithID(string PackID)
        {
            string text = "DELETE FROM \"Pack\" WHERE \"Pack\".PackID = @pi;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue($"pi", PackID);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool DeleteAllPacks()
        {
            string text = "DELETE FROM \"Pack\" CASCADE;";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
