using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    public static class FightCalculations
    {
        public static int Compare(CardInstance Card1, CardInstance Card2)
        {
            List<string> Outputs = new();
            string string1;
            string string2;
            string StringWhole;

            int BasePower1 = Card1.BaseCard.Power;
            int BasePower2 = Card2.BaseCard.Power;


            double RatingMultiplier1 = Math.Round(1 + ((double)Card1.Rating)/100, 2);
            double RatingMultiplier2 = Math.Round(1 + ((double)Card2.Rating)/100, 2);

            double ElementMultiplier1 = ElementMultiplier(Card1.BaseCard, Card2.BaseCard);
            double ElementMultiplier2 = ElementMultiplier(Card2.BaseCard, Card1.BaseCard);

            double FactionMultiplier1 = FactionMultiplier(Card1.BaseCard, Card2.BaseCard);
            double FactionMultiplier2 = FactionMultiplier(Card2.BaseCard, Card1.BaseCard);


            string[] Table = new string[3];

            AddTableRow(Table, "Power", $"{BasePower1}", $"{BasePower2}");
            AddTableRow(Table, "Rating", $"x{RatingMultiplier1}", $"x{RatingMultiplier2}");
            AddTableRow(Table, "Element", $"x{ElementMultiplier1}", $"x{ElementMultiplier2}");
            AddTableRow(Table, "Faction", $"x{FactionMultiplier1}", $"x{FactionMultiplier2}");

            int totalPower1 = (int)Math.Floor(BasePower1 * RatingMultiplier1 * ElementMultiplier1 * FactionMultiplier1);
            int totalPower2 = (int)Math.Floor(BasePower2 * RatingMultiplier2 * ElementMultiplier2 * FactionMultiplier2);

            AddTableRow(Table, "Total", $"={totalPower1}", $"={totalPower2}");

            Outputs.Add(Table[0]);
            Outputs.Add(Table[1]);
            Outputs.Add(Table[2]);

            int Winner = (totalPower1 > totalPower2) ? 1 : 2;
            if (totalPower1 == totalPower2) Winner = 0;

            foreach(string Output in Outputs){
                Console.WriteLine(Output);
            }

            return Winner;
        }

        public static string[] AddTableRow(string[] InputString, string Header, string Input1, string Input2)
        {
            int lh = Header.Length;
            int i1h = Input1.Length;
            int i2h = Input2.Length;

            int length = GetBiggestNumber(lh, i1h, i2h) + 1;

            InputString[0] += fillStringWithSpaces(Header, length) + " | ";
            InputString[1] += fillStringWithSpaces(Input1, length) + " | ";
            InputString[2] += fillStringWithSpaces(Input2, length) + " | ";

            return InputString;
        }

        public static string fillStringWithSpaces(string input, int length)
        {
            for(int i = input.Length; i < length; i++)
            {
                input += " ";
            }
            return input;
        }

        public static int GetBiggestNumber(int a, int b, int c)
        {
            if (a <= b)
            {
                if (b <= c) return c;
                else return b;
            } else
            {
                if (a <= c) return c;
                else return a;
            }
        }

        public static double ElementMultiplier(CardTemplate Attacker, CardTemplate Defender)
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
        public static double FactionMultiplier(CardTemplate Attacker, CardTemplate Defender)
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
