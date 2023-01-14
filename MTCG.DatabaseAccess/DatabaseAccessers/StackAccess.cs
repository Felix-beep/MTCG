using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class StackAccess
    {
        public static bool CreateStack(string Username, List<string> Cards)
        {
            if(Cards.Count == 0) return false;
            string text = "INSERT INTO \"Stack\" (\"Username\", \"CardId\") VALUES";
            for (int i = 1; i <= Cards.Count; i++)
            {
                Console.WriteLine(i-1+ ": " + Cards[i-1]);
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
            Console.WriteLine("- User = '" + Username + "'");
            string text = @"SELECT ""Stack"".""Username"", 
                            (""CardTemplate"".""Power"" * ( 1 + ""CardInstance"".""Rating""::float / 100))::int AS EffectivePower, 
                            ""CardInstance"".""Rating"", ""CardInstance"".""CardID"", 
                            ""CardTemplate"".""Cardname"", ""CardTemplate"".""Power"", ""CardTemplate"".""Type"",  ""CardTemplate"".""Faction"",  ""CardTemplate"".""Element""
                            FROM ""Stack"" 
                            INNER JOIN ""CardInstance"" ON ""CardInstance"".""CardID"" = ""Stack"".""CardId"" 
                            INNER JOIN ""CardTemplate"" ON ""CardTemplate"".""Cardname"" = ""CardInstance"".""Cardname"" WHERE ""Stack"".""Username"" = @u1 
                            ORDER BY 2 DESC";

            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u1", Username);
            var reader = DatabaseAccess.GetReader(command);
            Stack Stack = new(Username);

            if (reader == null) return null;
            
            if (!reader.HasRows)
            {
                Console.Write("Has 0 Rows");
                reader.Close();
                return Stack;
            }
            Console.WriteLine("Found cards");
            while (reader.Read())
            {
                int Rating, Power;
                string CardID, CardName, Type, Element, Faction;
                try
                {
                    Rating = reader.GetInt32(2);
                    CardID = reader.GetString(3);
                    CardName = reader.GetString(4);
                    Power = reader.GetInt32(5);
                    Type = reader.GetString(6);
                    Faction = reader.GetString(7);
                    Element = reader.GetString(8);
                } catch (Exception ex)
                {
                    Console.WriteLine("Error reading from Database: " + ex.Message);
                    reader.Close();
                    return null;
                }

                CardTemplate BaseCard = new(CardName, Power, Element, Type, Faction);
                CardInstance CardInstance = new(Rating, CardName, CardID, BaseCard);
                Console.WriteLine("Adding Cards to Stack");
                Stack.CardList.Add(CardInstance);
            }
            reader.Close();
            return Stack;
        }
        public static bool FindCardInStack(string Username, string CardID)
        {
            Console.WriteLine("-- Looking for card [{CardID}] on User [{Username}]");
            string text =   "SELECT \"Username\", \"CardId\" ";
            text +=         "FROM \"Stack\" ";
            text +=         "WHERE \"CardId\" = @ci ";
            text +=         "AND \"Username\" = @u ";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("ci", CardID);
            command.Parameters.AddWithValue("u", Username);
            var reader = DatabaseAccess.GetReader(command);
            if (reader == null || !reader.HasRows) 
            {
                reader.Close();
                Console.WriteLine("-- Card not found.");
                return false; 
            }
            Console.WriteLine("-- Card found.");
            reader.Close();
            return true;
        }

        public static bool DeleteFromStack(string Username, string cardId)
        {
            string text = "DELETE FROM \"Stack\" WHERE \"USERNAME\" = @u AND \"CardID\" = @ci";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue($"u", Username);
            command.Parameters.AddWithValue($"ci", cardId);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool AddToStack(string Username, List<string> Cards)
        {
            return CreateStack(Username, Cards);
        }

        public static bool DeleteAllStacks()
        {
            string text = "DELETE FROM \"Stack\" CASCADE;";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
