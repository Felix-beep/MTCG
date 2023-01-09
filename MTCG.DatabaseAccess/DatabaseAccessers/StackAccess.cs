using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return DatabaseAccess.GetWriter(command);
        }

        public static User GetStack(string Username)
        {
            string text = "SELECT * FROM \"User\" WHERE \"Username\" = @u JOIN \"CardInstance\" ON \"Cardid\"";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("@u", Username);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;

            List<string> CardIds = new();
            while (reader.Read())
            {
                
            }
            reader.Read();
            string Name = reader.GetString(0);
            string Password = reader.GetString(1);
            string Bio = reader?.GetString(2);
            string Picture = reader?.GetString(3);
            int Gold = reader.GetInt16(4);

            return new User(Username, Password, Bio, Picture, Gold);
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
