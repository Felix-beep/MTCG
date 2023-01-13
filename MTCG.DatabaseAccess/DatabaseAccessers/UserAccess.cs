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
            string text = "INSERT INTO \"User\" VALUES ( @u, @p )";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            command.Parameters.AddWithValue("p", Password);
            return DatabaseAccess.GetWriter(command);
        }
        
        public static User GetUser(string Username)
        {
            string text = "SELECT * FROM \"User\" WHERE \"Username\" = @u ";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return null;
            reader.Read();
            if(!reader.HasRows) { 
                reader.Close(); 
                return null; 
            }
            
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

        public static bool EditUser(string Username, string Bio, string Picture)
        {
            string text = "UPDATE \"User\" SET \"Bio\" = @b, \"Picture\" = @p WHERE \"Username\" = @u ;";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("b", Bio);
            command.Parameters.AddWithValue("p", Picture);
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
