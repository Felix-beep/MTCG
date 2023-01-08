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
        static void Main(string[] args) 
        {
            bool boolfound = false;

            string ip = "127.0.0.1";
            string port = "5432";
            string user = "postgre";
            string password = "DY8q6EGDEfUTW7KYCJsY";
            string database = "MTCGDatabase";

            using (NpgsqlConnection conn = new NpgsqlConnection($"Server={ip}; Port={port}; User Id={user}; Password={password}; Database={database}"))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Table1", conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    boolfound = true;
                    Console.WriteLine("connection established");
                }
                if (boolfound == false)
                {
                    Console.WriteLine("Data does not exist");
                }
                dr.Close();
            }
        }
    }
}
