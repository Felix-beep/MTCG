using MTCG.Backend;
using MTCG.BL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    internal static class Shop
    { 
        public static bool BuyPack(User buyer)
        {
            if(buyer.Gold >= 5)
            {
                Pack RandomPack = PackHandler.GetRandomPack();
                if (RandomPack != null)
                {
                    buyer.Gold = buyer.Gold - 5;
                    buyer.AddPack(RandomPack);
                    return true;
                }
            }
            return false;
        }

        public static bool SellCard(CardInstance cardToSell, int goldCost, User owner)
        {
            TradeOffer SellEntry = new TradeOffer(cardToSell, goldCost, owner);
            TradeOfferBase.AddOffer(SellEntry);
            return SellEntry.Valid;
        }

        public static bool BuyCard(TradeOffer cardToBuy, User buyer)
        {
            return cardToBuy.buyCard(buyer);
        }

        public static List<TradeOffer> GetOffers()
        {
            return TradeOfferBase.TradeOffers;
        }
    }
}
