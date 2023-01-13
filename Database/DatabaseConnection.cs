using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database
{
    public static class DatabaseConnection
    {
        public static NpgsqlConnection connection = null;
        public static NpgsqlConnection Connection
        {
            get
            {
                if (connection == null) OpenConnection();
                return connection;
            }
            private set
            {
                connection = value;
            }
        }

        public static void OpenConnection() 
        {
            string ip = "127.0.0.1";
            string port = "5432";
            string user = "postgres";
            string database = "MTCGDatabase";

            NpgsqlConnection conn = new NpgsqlConnection($"Server={ip}; Port={port}; User Id={user}; Database={database}");

            conn.Open();

            if(conn.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Successfully opened connection");
                connection = conn;
                return;
            }
            connection = null;
            return;
        }
    }
}
