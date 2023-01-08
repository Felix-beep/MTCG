using MTCG.Models;
using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTCG.BL
{
    public static class ShopHandler
    {
        public static CurlResponse CreateTrade(string Username, string Id, string CardId, string Name, int Rating)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse BuyTrade(string Id)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse DeleteTrade(string Id)
        {
            CurlResponse response = new();

            return response;
        }

        public static CurlResponse GetTradeDeals()
        {
            CurlResponse response = new();

            return response;
        }
    }
}
