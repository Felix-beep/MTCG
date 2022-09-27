using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    internal class CardTemplate
    {
        public string Name { get; }
        public int Power { get; }
        public Elements Element { get; }
        public Types Type { get; }
        public Factions Faction { get; }

        public CardTemplate(string name, int power, Elements element, Types type, Factions faction)
        {
            Name = name;
            Element = element;
            Power = power;
            Type = type;
            Faction = faction;
        }

        public void printCard()
        {
            Console.WriteLine($"Hello, I am a {Name}");
        }

    }
}
