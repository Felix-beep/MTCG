﻿using MTCG.Models;
using MTCG.MODELS;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            for (int i = 1; i <= Cards.Count; i++)
            {
                command.Parameters.AddWithValue($"u{i}", Username);
                command.Parameters.AddWithValue($"p{i}", Cards[i-1]);
            }
            return DatabaseAccess.GetWriter(command);
        }

        public static Stack GetStack(string Username)
        {
            string text = "SELECT \"Stack\".\"Username\", ";
            text +=         "\"CardInstance\".\"Rating\", \"CardInstance\".\"CardID\", ";
            text +=         "\"CardTemplate\".\"Cardname\", \"CardTemplate\".\"Power\", \"CardTemplate\".\"Type\",  \"CardTemplate\".\"Faction\" ";
            text +=         "FROM \"STACK\" ";
            text +=         "INNER JOIN \"CardInstance\" ON \"CardInstance\".\"CardID\" = \"Stack\".\"CardId\" ";
            text +=         "INNER JOIN \"CardTemplate\" ON \"CardTemplate\".\"Cardname\" = \"CardInstance\".\"Cardname\" ";
            text +=         "WHERE \"Stack\".\"Username\" = @u";
            text +=         "ORDER BY \"Stack\".\"Id \"";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("u", Username);
            var reader = DatabaseAccess.GetReader(command);
            Stack Stack = new(Username);

            if (reader == null) return null;
            if (!reader.HasRows)
            {
                reader.Close();
                return Stack;
            }
            while (reader.Read())
            {
                int Rating, Power;
                string CardID, Name, Type, Element, Faction;
                try
                {
                    Rating = reader.GetInt32(1);
                    CardID = reader.GetString(2);
                    Name = reader.GetString(3);
                    Power = reader.GetInt32(4);
                    Type = reader.GetString(5);
                    Element = reader.GetString(6);
                    Faction = reader.GetString(7);
                } catch
                {
                    Console.WriteLine("Error reading from Database.");
                    reader.Close();
                    return null;
                }

                CardTemplate BaseCard = new(Name, Power, Element, Type, Faction);
                CardInstance CardInstance = new(Rating, Name, CardID, BaseCard);
                Stack.CardList.Add(CardInstance);
            }
            reader.Close();
            return Stack;
        }
        public static bool FindCardInStack(string Username, string CardID)
        {
            string text =   "SELECT \"Username\", \"CardId\" ";
            text +=         "FROM \"STACK\" ";
            text +=         "WHERE \"CardId\" = @ci ";
            text +=         "AND \"Username\" = @u ";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue("ci", CardID);
            command.Parameters.AddWithValue("u", Username);
            var reader = DatabaseAccess.GetReader(command);

            if (reader == null) return false;
            return true;
        }

        public static bool DeleteFromStack(string Username, string cardId)
        {
            string text = "DELETE FROM \"Stack\" WHERE \"USERNAME\" = @u AND \"CardID\" = @ci";
            var command = new NpgsqlCommand(text);
            command.Parameters.AddWithValue($"u", Username);
            command.Parameters.AddWithValue($"ci", cardId);
            return DatabaseAccess.GetWriter(command);
        }

        public static bool AddToStack(string Username, List<string> Cards)
        {
            return CreateStack(Username, Cards);
        }

        public static bool DeleteAllStacks()
        {
            string text = "DELETE FROM \"Stack\" CASCADE;";
            var command = new NpgsqlCommand(text);
            return DatabaseAccess.GetWriter(command);
        }
    }
}
