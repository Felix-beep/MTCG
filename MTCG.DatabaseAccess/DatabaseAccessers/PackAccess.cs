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
        public static bool CreatePack(string Username, List<string> CardNames, List<string> CardIDs)
        {
            if (CardNames.Count == 0) return false;
            string text = "INSERT INTO \"Pack\" (\"Username\", \"Cardname\", \"PackID\", \"CardID\") VALUES";
            for (int i = 1; i <= CardNames.Count; i++)
            {
                text += $" (@u{i}, @cn{i}, @ti{i}, @ci{i})";
                if (i != CardNames.Count)
                {
                    text += ",";
                }
            }
            var command = new NpgsqlCommand(text);
            string PackId = Guid.NewGuid().ToString();
            for (int i = 1; i <= CardNames.Count; i++)
            {
                command.Parameters.AddWithValue($"u{i}", Username);
                command.Parameters.AddWithValue($"cn{i}", CardNames[i - 1]);
                command.Parameters.AddWithValue($"ti{i}", PackId);
                command.Parameters.AddWithValue($"ci{i}", CardIDs[i - 1]);
            }
            return DatabaseAccess.GetWriter(command);
        }

        public static Tuple<string, List<CardInstance>> GetPack()
        {
            string text = "SELECT ct.\"Cardname\", ct.\"Power\", ct.\"Type\", ct.\"Element\", ct.\"Faction\", p.\"PackID\", p.\"CardID\" ";
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
                string Name, Type, Element, Faction, CardID;
                int Power;
                try
                {
                    Name = reader.GetString(0);
                    Power = reader.GetInt32(1);
                    Type = reader.GetString(2);
                    Element = reader.GetString(3);
                    Faction = reader.GetString(4);
                    CardID = reader.GetString(6);
                } catch
                {
                    Console.WriteLine("Error reading from Database.");
                    reader.Close();
                    return null;
                }

                // get first PackID that was found
                if(PackID == "") PackID = reader.GetString(5);

                CardTemplate BaseCard = new(Name, Power, Element, Type, Faction);
                CardInstance CardInstance = new(BaseCard, CardID);
                Cards.Add(CardInstance);
            }
            reader.Close();

            if(Cards.Count != 4)
            {
                if (!DeleteAllPacksWithID(PackID))
                {
                    Console.WriteLine("Error when deleting pack by ID!");
                    return null;
                }
                return GetPack();
            }

            return new Tuple<string, List<CardInstance>> (PackID, Cards);
        }
        public static bool DeleteAllPacksWithID(string PackID)
        {
            string text = "DELETE FROM \"Pack\" WHERE \"PackID\" = @pi;";
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
