using MTCG.Models;
using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MTCG.Backend
{
    public static class CardBase
    {
        private static Dictionary<String, CardTemplate> cardList;

        public static bool cardsLoaded = false;

        public static CardTemplate getCard(String CardName)
        {
            return cardList[CardName];
        }

        public static List<CardTemplate> DicToList()
        {
            return Enumerable.ToList(cardList.Values);
        }

        public static IEnumerable<CardTemplate> getRandomCard()
        {
            List<CardTemplate> cards = DicToList();
            Random r = new Random();
            while (true)
            {
                yield return cards[r.Next(cards.Count)];
            }

        }

        public static void LoadCards()
        {
            cardsLoaded = true;
            // Fire 
            cardList.Add("Goblin Lackey", new CardTemplate("Goblin Lackey", 15, Elements.Fire, Types.Monster, Factions.Goblin));
            cardList.Add("Goblin Matron", new CardTemplate("Goblin Matron", 25, Elements.Fire, Types.Monster, Factions.Goblin));
            cardList.Add("Goblin Warchief", new CardTemplate("Goblin Warchief", 35, Elements.Fire, Types.Monster, Factions.Goblin));

            cardList.Add("Lava Hound", new CardTemplate("Lava Hound", 25, Elements.Fire, Types.Monster, Factions.NoFaction));

            cardList.Add("Balefire Dragon", new CardTemplate("Balefire Dragon", 40, Elements.Fire, Types.Monster, Factions.Dragon));

            cardList.Add("Roast", new CardTemplate("Roast", 20, Elements.Fire, Types.Spell, Factions.Goblin));
            cardList.Add("Rain Lava", new CardTemplate("Rain Lava", 30, Elements.Fire, Types.Spell, Factions.NoFaction));

            // Nature
            cardList.Add("Wood Elves", new CardTemplate("Wood Elves", 20, Elements.Nature, Types.Monster, Factions.Elf));
            cardList.Add("Big Game Huntress", new CardTemplate("Big Game Huntress", 30, Elements.Nature, Types.Monster, Factions.Elf));
            cardList.Add("Evlish Archdruid", new CardTemplate("Evlish Archdruid", 35, Elements.Nature, Types.Monster, Factions.Elf));

            cardList.Add("Oakheart Dryad", new CardTemplate("Elf", 25, Elements.Nature, Types.Monster, Factions.NoFaction));

            cardList.Add("Primordial Hydra", new CardTemplate("Primordial Hydra", 40, Elements.Nature, Types.Monster, Factions.Hydra));

            cardList.Add("Tangletrap", new CardTemplate("Tangletrap", 20, Elements.Nature, Types.Spell, Factions.Elf));
            cardList.Add("Naturalize", new CardTemplate("Naturalize", 30, Elements.Nature, Types.Spell, Factions.NoFaction));

            // Water
            cardList.Add("River Mermaid", new CardTemplate("River Mermaid", 20, Elements.Water, Types.Monster, Factions.Mermaid));
            cardList.Add("Mermaid Trickster", new CardTemplate("Mermaid Trickster", 30, Elements.Water, Types.Monster, Factions.Mermaid));
            cardList.Add("Shipwreck Mermaid", new CardTemplate("Shipwreck Mermaid", 35, Elements.Water, Types.Monster, Factions.Mermaid));

            cardList.Add("Whitewater Naiad", new CardTemplate("Whitewater Naiad", 25, Elements.Water, Types.Monster, Factions.NoFaction));

            cardList.Add("Shipbreaker Kraken", new CardTemplate("Shipbreaker Kraken", 40, Elements.Water, Types.Monster, Factions.Kraken));

            cardList.Add("Water Spear", new CardTemplate("Water Spear", 20, Elements.Water, Types.Spell, Factions.Mermaid));
            cardList.Add("Water Vortex", new CardTemplate("Water Vortex", 30, Elements.Water, Types.Spell, Factions.NoFaction));
        }
    }
}
