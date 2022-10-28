using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class TradeOffer
    {
        public CardInstance CardToSell { get; }
        public int GoldCost { get; }
        public User Owner { get; }
        public bool Valid { get; }
        public bool ID { get; }

        public TradeOffer(CardInstance cardToSell, int goldCost, User owner)
        {
            CardToSell = cardToSell;
            GoldCost = goldCost;
            Owner = owner;
            Valid = owner.RemoveCard(CardToSell);
        }

        public void printCard()
        {
            Console.WriteLine($"{CardToSell.BaseCard.Name} Power: {CardToSell.EffectivePower} Rating: {CardToSell.Rating} Cost: {GoldCost}");
        }

        public void printInfo()
        {
            CardToSell.Print();
            Console.WriteLine($"Cost: {GoldCost}");
        }

        public bool buyCard(User buyer)
        {
            if(buyer.Gold - GoldCost >= 0)
            {
                buyer.Gold -= GoldCost;
                buyer.AddCard(CardToSell);
                return true;
            }
            return false;
        }
    }
}
