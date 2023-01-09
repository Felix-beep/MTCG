using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database
{
    public class DatabaseConnection
    {
        public static NpgsqlConnection Connection { get; set; } = null;

        public void OpenConnection() 
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
                Connection = conn;
            }
        }

        public DatabaseConnection() { 
            if(Connection == null)
            {
                OpenConnection();
            }
        }

        ~DatabaseConnection()
        {
            if(Connection != null)
            {
                Connection.Close();
                Connection = null;
            }
        }
    }
}
