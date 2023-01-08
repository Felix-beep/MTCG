using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Backend
{
    public static class TradeOfferBase
    {
        private static List<TradeOffer> tradeOffers = new List<TradeOffer>();

        public static List<TradeOffer> TradeOffers { get { return tradeOffers; } }

        public static bool AddOffer(TradeOffer toAdd)
        {
            if (toAdd.Valid) { 
                tradeOffers.Add(toAdd);
                return true;
            }
            return false;
        }
    }
}
