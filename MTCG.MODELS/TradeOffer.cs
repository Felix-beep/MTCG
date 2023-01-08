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
    }
}
