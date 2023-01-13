using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class CardTemplateAccess
    {
        public static bool CreateCardTemplate(string Cardname, int Power, string Type, string Faction, string Element)
        {
            string text = "INSERT INTO \"CardTemplate\" VALUES ( @cn, @p, @t, @f, @e )";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("cn", Cardname);
            command.Parameters.AddWithValue("p", Power);
            command.Parameters.AddWithValue("t", Type);
            command.Parameters.AddWithValue("f", Faction);
            command.Parameters.AddWithValue("e", Element);
            return DatabaseAccess.GetWriter(command);
        }

        public static CardTemplate GetCardTemplate(string CardName)
        {
            string text = "SELECT * FROM \"CardTemplate\" WHERE \"Cardname\" = @cn ";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("cn", CardName);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            reader.Read();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }

            string Name, Type, Faction, Element;
            int Power;

            try {
                Name = reader.GetString(0);
                Power = reader.GetInt32(1);
                Type = reader.GetString(2);
                Faction = reader.GetString(3);
                Element = reader.GetString(4);
            } catch
            {
                Console.WriteLine("Error reading from Database");
                reader.Close();
                return null;
            }
            reader.Close();

            return new CardTemplate(Name, Power, Type, Faction, Element);
        }

        public static bool DeleteAllCardTemplates()
        {
            string text = "DELETE FROM \"CardTemplate\" CASCADE;";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
