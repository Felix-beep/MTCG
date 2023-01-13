using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class CardTemplate
    {
        public string Name { get; }
        public int Power { get; }
        public Elements Element { get; }
        public Types Type { get; }
        public Factions Faction { get; }

        public CardTemplate(string name, int power, string element, string type, string faction)
        {
            Name = name;
            switch (element)
            {
                case "Water": Element = Elements.Water; break;
                case "Fire": Element = Elements.Water; break;
                case "Nature": Element = Elements.Water; break;
                default: Element = Elements.NoElement; break;
            }
            switch (type)
            {   
                case "Spell": Type = Types.Spell; break;
                case "Monster": Type = Types.Monster; break;
                default: Type = Types.NoType; break;
            }
            switch (faction)
            {
                case "Goblin": Faction = Factions.Goblin; break;
                case "Elf": Faction = Factions.Elf; break;
                case "Mermaid": Faction = Factions.Mermaid; break;
                case "Dragon": Faction = Factions.Dragon; break;
                case "Kraken": Faction = Factions.Kraken; break;
                case "Hydra": Faction = Factions.Hydra; break;
                default: Faction = Factions.NoFaction; break;
            }
            Power = power;
        }
    }
}
