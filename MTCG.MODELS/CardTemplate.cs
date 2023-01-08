using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class CardTemplate : IPrintable
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

        public CardTemplate()
        {
            Name = "EmptyCardtemplate";
            Power = 0;
            Element = Elements.NoElement;
            Type = Types.NoType;
            Faction = Factions.NoFaction;
        }

        public override void Print()
        {
            Console.WriteLine($"[{Name}]");
            Console.WriteLine($"[Type: {Type}] [Faction: {Faction}] [Element: {Element}]\n");
        }
    }
}
