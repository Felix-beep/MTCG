using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTCG.Models
{
    public class CardInstance
    {
        public CardTemplate ThisCard { get; }
        public int Rating { get; }

        public CardInstance(CardTemplate thisCard)
        {
            ThisCard = thisCard;
            Random r = new Random();
            Rating = r.Next(0, 100);
        }

        public void printCard()
        {
            Console.WriteLine($"Hello, I am a {ThisCard.Name} with a rating of {Rating}%");
        }
    }
}
