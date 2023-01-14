using Npgsql.Replication.PgOutput.Messages;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.MODELS;
using System.Data;

namespace MTCG.DatabaseAccess.DatabaseAccessers
{
    public static class UserAccess
    {
        public static bool CreateUser(string Username, string Password)
        {
            Console.WriteLine($"-- Creating User [{Username}] Password [{Password}]");
            string text = "INSERT INTO \"User\" VALUES ( @u, @p )";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            command.Parameters.AddWithValue("p", Password);
            return DatabaseAccess.GetWriter(command);
        }
        
        public static User GetUser(string Username)
        {
            Console.WriteLine($"-- Getting User [{Username}]");
            string text = "SELECT * FROM \"User\" WHERE \"Username\" = @u ";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            reader.Read();
            
            
            string Name, Password, Bio, Picture;
            int Gold;

            try
            {
                Name = reader.GetString(0);
                Password = reader.GetString(1);
                Bio = reader?.GetString(2);
                Picture = reader?.GetString(3);
                Gold = reader.GetInt16(4);
            } catch
            {
                Console.WriteLine("Error reading from Database.");
                reader.Close();
                return null;
            }
            reader.Close();

            return new User(Username, Password, Bio, Picture, Gold); 
        }

        public static bool EditUser(string User, string Username, string Bio, string Picture)
        {
            Console.WriteLine($"-- Setting User [{User}] Username = [{Username}] to Bio = [{Bio}] and Picture = [{Picture}]");
            string text = "UPDATE \"User\" SET \"Username\" = @un, \"Bio\" = @b, \"Picture\" = @p WHERE \"Username\" = @u ;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", User);
            command.Parameters.AddWithValue("b", Bio);
            command.Parameters.AddWithValue("p", Picture);
            command.Parameters.AddWithValue("un", Username);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool ChangeUserGold(string Username, int Gold)
        {
            Console.WriteLine($"-- Setting User [{Username}] to Gold = [{Gold}]");
            string text = "UPDATE \"User\" SET \"Gold\" = @g WHERE \"Username\" = @u ;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("g", Gold);
            command.Parameters.AddWithValue("u", Username);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool DeleteAllUsers()
        {
            string text = "DELETE FROM \"User\" CASCADE;";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
