using Npgsql;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MTCG.Database
{
    public static class DatabaseInitialiser
    {
        public static string DatabaseName = "MTCGDatabase";
        static void Main()
        {
            bool abort = false;
            int input = 0;
            NpgsqlConnection Connection = null;

            while (abort == false)
            {
                Console.WriteLine("\nMenue:\n[0] to abort [1] to create Database [2] to drop Database [3] to reset all Tables");
                Console.Write(">");
                string userInput = Console.ReadLine();
                int num = 10;
                try
                {
                    num = Convert.ToInt32(userInput);
                } catch 
                {
                    num = 10;
                }

                switch (num)
                {
                    case 0: abort = true; break;
                    case 1:
                        Connection = GetPostgreSQLConnection();

                        Connection.Open();
                        InitDatabase(Connection);
                        Connection.Close();

                        Connection = GetDatabaseConnection();

                        Connection.Open();
                        CreateSequences(Connection);

                        Connection = GetDatabaseConnection();

                        Connection.Open();
                        CreateTables(Connection);

                        Connection = GetDatabaseConnection();
                        Connection.Open();

                        AddBaseValues(Connection);
                        Connection.Close();
                        break;
                    case 2:
                        Connection = GetPostgreSQLConnection();
                        Connection.Open();
                        DropDatabase(Connection);
                        Connection.Close();
                        abort = true;
                        break;
                    case 3:
                        Connection = GetDatabaseConnection();
                        Connection.Open();
                        ClearAllTables(Connection);
                        Connection = GetDatabaseConnection();
                        Connection.Open();
                        AddBaseValues(Connection);
                        Connection.Close();
                        break;
                    default: Console.WriteLine("Invalid Input!"); break;
                }
            }
        }

        public static NpgsqlConnection GetPostgreSQLConnection()
        {
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;";
            NpgsqlConnection localConnection = new NpgsqlConnection(connectionString);
            return localConnection;
        }
        public static NpgsqlConnection GetDatabaseConnection()
        {
            string connectionString = $"Server=localhost;Port=5432;User Id=postgres;Database={DatabaseName};";
            NpgsqlConnection localConnection = new NpgsqlConnection(connectionString);
            return localConnection;
        }

        public static void InitDatabase(NpgsqlConnection Connection)
        {
            using (Connection)
            {
                Console.WriteLine("\nCreating Database...");
                using (var Command = new NpgsqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{DatabaseName}';";
                    object objresult = null;
                    try
                    {
                        objresult = Command.ExecuteScalar();
                    } catch (Exception ex) 
                    {
                        Console.WriteLine($"Error when checking for Database: {ex.Message}");
                        return;
                    }
                    int count = Convert.ToInt32(objresult);

                    if (count == 1)
                    {
                        Console.WriteLine("Database already exists.");
                    }
                    else
                    {
                        Console.WriteLine("- Creating new Database.");
                        Command.CommandText = $"CREATE DATABASE \"{DatabaseName}\" WITH OWNER = postgres ENCODING = 'UTF8' LC_COLLATE = 'C' LC_CTYPE = 'C' TABLESPACE = pg_default CONNECTION LIMIT = -1 IS_TEMPLATE = False;";
                        int result;
                        try
                        {
                            result = Command.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error when creating Database: {ex.Message}");
                            return;
                        }
                        if (result == -1)
                        {
                            Console.WriteLine($"Succesfully created Database.");
                        }
                    }
                }
            }
        }

        public static void CreateSequences(NpgsqlConnection Connection)
        {
            using (Connection)
            {
                Console.WriteLine("\nCreating Sequences...");
                string commandString =
                  @"CREATE SEQUENCE IF NOT EXISTS ""CardInstance_ID_seq"";"
                + @"CREATE SEQUENCE IF NOT EXISTS ""CardTemplate_ID_seq"";"
                + @"CREATE SEQUENCE IF NOT EXISTS ""Deck_ID_seq"";"
                + @"CREATE SEQUENCE IF NOT EXISTS ""Leaderboard_ID_seq"";"
                + @"CREATE SEQUENCE IF NOT EXISTS ""Pack_ID_seq"";"
                + @"CREATE SEQUENCE IF NOT EXISTS ""Stack_ID_seq"";"
                + @"CREATE SEQUENCE IF NOT EXISTS ""Stats_ID_seq"";";

                using (var Command = new NpgsqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = commandString;
                    int result;
                    try
                    {
                        result = Command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error when adding Sequences to Database: {ex.Message}");
                        return;
                    }

                    if (result == -1)
                    {
                        Console.WriteLine($"Succesfully added Sequences to Database.");
                    }
                }
            }
        }

        public static void CreateTables(NpgsqlConnection Connection)
        {
            using (Connection)
            {
                Console.WriteLine("\nCreating Tables...");
                string commandString =
                @"CREATE TABLE IF NOT EXISTS public.""CardTemplate""
                    (
                        ""Cardname"" text COLLATE pg_catalog.""default"" NOT NULL,
                        ""Power"" integer NOT NULL,
                        ""Type"" text COLLATE pg_catalog.""default"" NOT NULL,
                        ""Faction"" text COLLATE pg_catalog.""default"" NOT NULL,
                        ""Element"" text COLLATE pg_catalog.""default"" NOT NULL,
                        ""ID"" integer NOT NULL DEFAULT nextval('""CardTemplate_ID_seq""'::regclass),
                        CONSTRAINT ""CardTemplate_pkey"" PRIMARY KEY (""ID""),
                        CONSTRAINT ""Carname"" UNIQUE (""Cardname"")
                    );"
                + @"CREATE TABLE IF NOT EXISTS public.""CardInstance""
                    (
                        ""ID"" integer NOT NULL DEFAULT nextval('""CardInstance_ID_seq""'::regclass),
                        ""Rating"" integer,
                        ""Cardname"" text COLLATE pg_catalog.""default"",
                        ""CardID"" text COLLATE pg_catalog.""default"",
                        CONSTRAINT ""CardInstance_pkey"" PRIMARY KEY (""ID""),
                        CONSTRAINT ""CardId"" UNIQUE (""CardID""),
                        CONSTRAINT ""Cardname"" FOREIGN KEY (""Cardname"")
                            REFERENCES public.""CardTemplate"" (""Cardname"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE
                    );"
                + @" CREATE TABLE IF NOT EXISTS public.""User""
                    (
                        ""Username"" text COLLATE pg_catalog.""default"" NOT NULL,
                        ""Password"" text COLLATE pg_catalog.""default"" NOT NULL,
                        ""Bio"" text COLLATE pg_catalog.""default"" DEFAULT 'No Bio'::text,
                        ""Picture"" text COLLATE pg_catalog.""default"" DEFAULT 'No Picture'::text,
                        ""Gold"" integer DEFAULT 20,
                        CONSTRAINT ""User_pkey"" PRIMARY KEY (""Username"")
                    );"
                + @"CREATE TABLE IF NOT EXISTS public.""Stack""
                    (
                        ""Username"" text COLLATE pg_catalog.""default"",
                        ""CardId"" text COLLATE pg_catalog.""default"",
                        ""Id"" integer NOT NULL DEFAULT nextval('""Stack_ID_seq""'::regclass),
                        CONSTRAINT ""Stack_pkey"" PRIMARY KEY (""Id""),
                        CONSTRAINT ""CardId"" FOREIGN KEY (""CardId"")
                            REFERENCES public.""CardInstance"" (""CardID"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE,
                        CONSTRAINT ""Username"" FOREIGN KEY (""Username"")
                            REFERENCES public.""User"" (""Username"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE
                    );"
                + @"CREATE TABLE IF NOT EXISTS public.""Deck""
                    (
                        ""Username"" text COLLATE pg_catalog.""default"",
                        ""ID"" integer NOT NULL DEFAULT nextval('""Deck_ID_seq""'::regclass),
                        ""CardID"" text COLLATE pg_catalog.""default"" NOT NULL,
                        CONSTRAINT ""Deck_pkey"" PRIMARY KEY (""ID""),
                        CONSTRAINT ""CardId"" FOREIGN KEY (""CardID"")
                            REFERENCES public.""CardInstance"" (""CardID"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE,
                        CONSTRAINT ""Username"" FOREIGN KEY (""Username"")
                            REFERENCES public.""User"" (""Username"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE
                    );"
                + @"CREATE TABLE IF NOT EXISTS public.""Pack""
                    (
                        ""Username"" text COLLATE pg_catalog.""default"",
                        ""Cardname"" text COLLATE pg_catalog.""default"",
                        ""ID"" integer NOT NULL DEFAULT nextval('""Pack_ID_seq""'::regclass),
                        ""PackID"" text COLLATE pg_catalog.""default"",
                        ""CardID"" text COLLATE pg_catalog.""default"",
                        CONSTRAINT ""Pack_pkey"" PRIMARY KEY(""ID""),
                        CONSTRAINT ""Cardname"" FOREIGN KEY(""Cardname"")
                            REFERENCES public.""CardTemplate"" (""Cardname"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE,
                        CONSTRAINT ""Username"" FOREIGN KEY(""Username"")
                            REFERENCES public.""User"" (""Username"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE,
                        CONSTRAINT ""CardID"" FOREIGN KEY (""CardID"")
                            REFERENCES public.""CardInstance"" (""CardID"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE
                    );"
                + @"CREATE TABLE IF NOT EXISTS public.""Tradeoffer""
                    (
                        ""Username"" text COLLATE pg_catalog.""default"",
                        ""CardId"" text COLLATE pg_catalog.""default"",
                        ""TradeId"" text COLLATE pg_catalog.""default"" NOT NULL,
                        ""Rating"" integer,
                        CONSTRAINT ""Tradeoffer_pkey"" PRIMARY KEY (""TradeId""),
                        CONSTRAINT ""CardId"" FOREIGN KEY (""CardId"")
                            REFERENCES public.""CardInstance"" (""CardID"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE,
                        CONSTRAINT ""Username"" FOREIGN KEY (""Username"")
                            REFERENCES public.""User"" (""Username"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE
                    );"
                + @"CREATE TABLE IF NOT EXISTS public.""Leaderboard""
                    (
                        ""Username"" text COLLATE pg_catalog.""default"",
                        ""Elo"" integer DEFAULT 1000,
                        ""RandomId"" integer NOT NULL DEFAULT nextval('""Leaderboard_ID_seq""'::regclass),
                        CONSTRAINT ""Leaderboard_pkey"" PRIMARY KEY (""RandomId""),
                        CONSTRAINT ""Username"" FOREIGN KEY (""Username"")
                            REFERENCES public.""User"" (""Username"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE
                    );"
                + @"CREATE TABLE IF NOT EXISTS public.""Stats""
                    (
                        ""Username"" text COLLATE pg_catalog.""default"",
                        ""Elo"" integer NOT NULL DEFAULT 1000,
                        ""Wins"" integer NOT NULL DEFAULT 0,
                        ""Losses"" integer NOT NULL DEFAULT 0,
                        ""ID"" integer NOT NULL DEFAULT nextval('""Stats_ID_seq""'::regclass),
                        CONSTRAINT ""Stats_pkey"" PRIMARY KEY (""ID""),
                        CONSTRAINT ""Username"" FOREIGN KEY (""Username"")
                            REFERENCES public.""User"" (""Username"") MATCH SIMPLE
                            ON UPDATE CASCADE
                            ON DELETE CASCADE
                    );";

                using (var Command = new NpgsqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = commandString;
                    int result;
                    try
                    {
                        result = Command.ExecuteNonQuery();
                    } catch (Exception ex )
                    {
                        Console.WriteLine($"Error when filling Database: {ex.Message}");
                        return;
                    }

                    if(result == -1)
                    {
                        Console.WriteLine($"Succesfully filled Database.");
                    }
                }
            }
        }

        public static void ClearAllTables(NpgsqlConnection Connection)
        {
            using (Connection)
            {
                Console.WriteLine("\nClearing All Tables...");
                string[] TableNames = { "CardInstance", "CardTemplate", "Deck", "Leaderboard", "Pack", "Stack", "Stats", "Tradeoffer", "User" };

                foreach (string table in TableNames)
                {
                    Console.WriteLine($"- Clearing Table {table}");
                    string commandString = $"DELETE FROM \"{table}\"";

                    using (var Command = new NpgsqlCommand())
                    {
                        Command.Connection = Connection;
                        Command.CommandText = commandString;
                        int result;
                        try
                        {
                            result = Command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error when adding Sequences to Database: {ex.Message}");
                            return;
                        }

                        if (result == -1)
                        {
                            Console.WriteLine($"Succesfully added Sequences to Database.");
                        }
                    }
                }
            }
        }

        public static void AddBaseValues(NpgsqlConnection Connection)
        {
            using (Connection)
            {
                Console.WriteLine("\nAdding Base Values...");
                string commandString =
                  @"INSERT INTO ""CardTemplate"" VALUES ( 'Goblin Lackey', 15, 'Fire', 'Monster', 'Goblin' );"
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Goblin Matron', 25, 'Fire', 'Monster', 'Goblin' );"
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Goblin Warchief', 30, 'Fire', 'Monster', 'Goblin'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Lava Hound', 25, 'Fire', 'Monster', 'NoFaction'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Balefire Dragon', 40, 'Fire', 'Monster', 'Dragon'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Roast', 20, 'Fire', 'Spell', 'Goblin'); "
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Lava Rain', 30, 'Fire', 'Spell', 'NoFaction'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Wood Elves', 20, 'Nature', 'Monster', 'Elf'); "
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Big Game Huntress', 25, 'Nature', 'Monster', 'Elf'); "
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Elvish Archdruid', 30, 'Nature', 'Monster', 'Elf'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Oakheart Dryad', 25, 'Nature', 'Monster', 'NoFaction'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Primordial Hydra',  40, 'Nature', 'Monster', 'Hydra'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Tangletrap', 20, 'Nature', 'Spell', 'Elf'); "
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Naturalize', 30, 'Nature', 'Monster', 'NoFaction'); "


                + @"INSERT INTO ""CardTemplate"" VALUES ( 'River Mermaid', 20, 'Water', 'Monster', 'Mermaid'); "
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Mermaid Trickster', 25, 'Water', 'Monster', 'Mermaid'); "
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Shipwreck Mermaid', 30, 'Water', 'Monster', 'Mermaid'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Whitewater Naiad', 25, 'Water', 'Monster', 'NoFaction'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Shipbreaker Kraken', 40, 'Water', 'Monster', 'Kraken'); "

                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Water Spear', 20, 'Water', 'Monster', 'Mermaid'); "
                + @"INSERT INTO ""CardTemplate"" VALUES ( 'Water Vortex', 30, 'Water', 'Monster', 'NoFaction'); ";

                using (var Command = new NpgsqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = commandString;
                    int result;
                    try
                    {
                        result = Command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error when adding Sequences to Database: {ex.Message}");
                        return;
                    }

                    if (result == -1)
                    {
                        Console.WriteLine($"Succesfully added Sequences to Database.");
                    }
                }
            }
        }

        public static void DropDatabase(NpgsqlConnection Connection)
        {
            using (Connection)
            {
                Console.WriteLine("\nDeleteting Database...");

                string commandString = @$"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '{DatabaseName}';";

                using (var Command = new NpgsqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = commandString;
                    int result;
                    try
                    {
                        result = Command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"- Error kicking from Database: {ex.Message}");
                        return;
                    }

                    if (result == -1)
                    {
                        Console.WriteLine($"- Succesfully kicked from Database.");
                    }
                }

                commandString = $"DROP DATABASE \"{DatabaseName}\"";

                using (var Command = new NpgsqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = commandString;
                    int result;
                    try
                    {
                        result = Command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error dropping Database: {ex.Message}");
                        return;
                    }

                    if (result == -1)
                    {
                        Console.WriteLine($"Succesfully dropped Database.");
                    }
                }

                
            }
        }
    }
}
