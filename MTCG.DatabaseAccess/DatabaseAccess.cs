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
        private static Mutex LockAccess = new Mutex();
        public static NpgsqlDataReader? GetReader(NpgsqlCommand Command)
        {
            Command.Prepare();
            NpgsqlDataReader? reader;
            LockAccess.WaitOne();
            try
            {
                Console.WriteLine(Command.CommandText);
                Command.Connection = DatabaseConnection.Connection;
                reader = Command.ExecuteReader();
                LockAccess.ReleaseMutex();
                return reader;
                
            } catch (Exception ex)
            {
                Console.WriteLine($"Error executing Command - ({ex})");
                reader = null;
            }
            LockAccess.ReleaseMutex();
            return reader;
        }

        public static bool GetWriter(NpgsqlCommand Command)
        {
            Command.Prepare();
            bool success;
            LockAccess.WaitOne();
            try
            {
                Console.WriteLine(Command.CommandText);
                Command.Connection = DatabaseConnection.Connection;
                Command.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing Command - ({ex})");
                success = false;
            }
            LockAccess.ReleaseMutex();

            return success;
        }
    }
}
