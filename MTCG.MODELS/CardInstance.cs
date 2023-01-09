using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace MTCG.MODELS
{
    public class CardInstance
    {
        public string CardName { get; }
        public int Rating { get; }
        public string ID { get; }


        public CardTemplate BaseCard { get; }
        public int EffectivePower { get; }

        public CardInstance(int Rating, string Cardname, string CardID, CardTemplate BaseCard)
        {
            this.CardName = Cardname;
            this.Rating = Rating;
            this.ID = CardID;
            this.BaseCard = BaseCard;
            this.EffectivePower = BaseCard.Power * (1 + Rating / 100);
        }
    }
}
