using MTCG.Database;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.DatabaseAccess.DatabaseAccessers;

namespace MTCG.DatabaseAccess
{
    public static class DatabaseAccess
    {

        public static NpgsqlDataReader GetReader(NpgsqlCommand Command)
        {
            DatabaseConnection myConnection = new DatabaseConnection();
            try
            {
                Console.WriteLine(Command.CommandText);
                Command.Connection = DatabaseConnection.Connection;
                var reader = Command.ExecuteReader();
                return reader;
                
            } catch (Exception ex)
            {
                Console.WriteLine($"Error executing Command - ({ex})");
                return null;
            }
        }

        public static bool GetWriter(NpgsqlCommand Command)
        {
            DatabaseConnection myConnection = new DatabaseConnection();
            try
            {
                Console.WriteLine(Command.CommandText);
                Command.Connection = DatabaseConnection.Connection;
                Command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing Command - ({ex})");
                return false;
            }
        }
    }
}
