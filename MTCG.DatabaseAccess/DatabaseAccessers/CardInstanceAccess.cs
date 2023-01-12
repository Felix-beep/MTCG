using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class CardInstaceAccess
    {
        public static bool CreateCardInstance(int Rating, string Cardname, string CardID)
        {
            string text = "INSERT INTO \"CardInstance\" (\"Rating\", \"Cardname\", \"CardID\") VALUES ( @r, @cn, @ci)";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("cn", Cardname);
            command.Parameters.AddWithValue("r", Rating);
            command.Parameters.AddWithValue("ci", CardID);
            return DatabaseAccess.GetWriter(command);
        }

        public static CardInstance GetCardInstance(string CardID)
        {
            string text = "SELECT * FROM \"CardInstance\" WHERE \"CardID\" = @ci ";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("ci", CardID);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            reader.Read();

            int Rating;
            string CardName;

            try
            {
                Rating = reader.GetInt32(1);
                CardName = reader.GetString(2);
            } catch
            {
                Console.WriteLine("Error reading from Database");
                reader.Close();
                return null;
            }
            reader.Close();

            CardTemplate BaseCard = CardTemplateAccess.GetCardTemplate(CardName);
            if (BaseCard == null) return null;

            return new CardInstance(Rating, CardName, CardID, BaseCard);
        }

        public static bool DeleteAllCardInstances()
        {
            string text = "DELETE FROM \"CardInstance\";";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
