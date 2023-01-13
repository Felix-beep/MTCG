using MTCG.Models;
using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    public static class FightControler
    {
        public static int Compare(CardInstance Card1, CardInstance Card2)
        {
            float PCard1 = Card1.EffectivePower * ElementMultiplier(Card1.BaseCard, Card2.BaseCard) * FactionMultiplier(Card1.BaseCard, Card2.BaseCard);
            float PCard2 = Card2.EffectivePower * ElementMultiplier(Card2.BaseCard, Card1.BaseCard) * FactionMultiplier(Card2.BaseCard, Card1.BaseCard);
            Console.WriteLine($"PlayerA: {Card1.BaseCard.Name} ({PCard1}) vs PlayerB: {Card2.BaseCard.Name} ({PCard2})");
            if ( PCard1 < PCard2 )
            {
                return 1;
            } else if (PCard1 > PCard2)
            {
                return -1;
            } else // (PCard1 == PCard2)
            {
                return 0;
            }
        }

        public static float ElementMultiplier(CardTemplate Attacker, CardTemplate Defender)
        {
            if(Attacker.Type == Types.Spell || Defender.Type == Types.Spell)
            {
                if(Attacker.Element == Elements.Fire)
                {
                    if(Defender.Element == Elements.Nature)
                        return 2;
                    if(Defender.Element == Elements.Water)
                        return 0.5f;
                }
                if (Attacker.Element == Elements.Water)
                {
                    if(Defender.Element == Elements.Fire)
                        return 2;
                    if (Defender.Element == Elements.Nature)
                        return 0.5f;
                }
                if (Attacker.Element == Elements.Nature)
                {
                    if (Defender.Element == Elements.Water)
                        return 2;
                    if (Defender.Element == Elements.Fire)
                        return 0.5f;
                }
            }
            return 1;
        }
        public static float FactionMultiplier(CardTemplate Attacker, CardTemplate Defender)
        {
            if (Attacker.Faction == Factions.Goblin && Defender.Faction == Factions.Dragon)
            {
                return 0;
            }
            if (Attacker.Type == Types.Spell && Defender.Faction == Factions.Kraken)
            {
                return 0;
            }
            if(Attacker.Faction == Factions.Elf && Defender.Faction == Factions.Dragon)
            {
                return 2;
            }
            if (Attacker.Faction == Factions.Goblin && Defender.Type == Types.Spell)
            {
                return 1.5f;
            }
            return 1;
        }
    }
}
