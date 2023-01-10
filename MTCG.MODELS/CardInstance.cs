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
            this.EffectivePower = CalculateEffectivePower();
        }

        public CardInstance(CardTemplate BaseCard) 
        {
            this.CardName = BaseCard.Name;
            Random r = new Random();
            this.Rating = r.Next(0, 101);
            Guid myuuid = Guid.NewGuid();
            this.ID = myuuid.ToString();
            this.BaseCard = BaseCard;
            this.EffectivePower = CalculateEffectivePower();
        }

        private int CalculateEffectivePower()
        {
            return BaseCard.Power * (1 + this.Rating / 100);
        }
    }
}
