﻿using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    public static class DeckHandler
    {
        public static CurlResponse CreateDeck(string Username, List<string> CardIds)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse GetDeck(string Username)
        {
            CurlResponse response = new();

            return response;
        }


        public static CurlResponse UpdateDeck(string Username, List<string> CardIds)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse ValidateDeck(string Username)
        {
            CurlResponse response = new();

            return response;
        }
    }
}