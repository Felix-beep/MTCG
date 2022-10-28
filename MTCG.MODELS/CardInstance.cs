using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace MTCG.MODELS
{
    public class CardInstance : IPrintable
    {
        public CardTemplate BaseCard { get; }
        public int Rating { get; }

        public int ID { get; }

        public int EffectivePower
        {
            get
            {
                double calc = BaseCard.Power * (1 + (double)Rating / 100);
                return (int)Math.Ceiling(calc);
            }
        }

        public CardInstance(CardTemplate thisCard)
        {
            BaseCard = thisCard;
            Random r = new Random();
            Rating = r.Next(0, 100);
            ID = r.Next(0, 1000000);
        }

        public CardInstance(CardTemplate thisCard, int rating)
        {
            BaseCard = thisCard;
            Rating = rating;
        }

        public override void Print()
        {
            Console.WriteLine($"[{BaseCard.Name}]");
            Console.WriteLine($"[Power: {BaseCard.Power} with a Rating of {Rating}% and Effective Power of {EffectivePower}]");
            Console.WriteLine($"[Type: {BaseCard.Type}] [Faction: {BaseCard.Faction}] [Element: {BaseCard.Element}]\n");
        }
    }
}
