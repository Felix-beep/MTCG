using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class DatabaseConnection
    {
        public static NpgsqlConnection Connection { get; set; }

        public static void OpenConnection() 
        {
            bool boolfound = false;

            string ip = "127.0.0.1";
            string port = "5432";
            string user = "postgres";
            string password = "DY8q6EGDEfUTW7KYCJsY";
            string database = "MTCGDatabase";

            NpgsqlConnection conn = new NpgsqlConnection($"Server={ip}; Port={port}; User Id={user}; Password={password}; Database={database}");

            conn.Open();

            if(conn.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Successfully opened connection");
                Connection = conn;
            } else
            {
                Connection = null;
            }
        }

        static void Main(string[] args)
        {
            OpenConnection();
        }
    }
}
