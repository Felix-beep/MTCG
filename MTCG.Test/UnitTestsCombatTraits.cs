using MTCG.BL;
using MTCG.Models;

namespace MTCG.Test
{
    public class UnitTestsCombatTraits
    {
        Dictionary<String, CardTemplate> CardList;

        [SetUp]
        public void Setup()
        {
            CardList = new Dictionary<string, CardTemplate>();
            //Goblin = new CardTemplate("Goblin", 20, Elements.Fire, Types.Monster, Factions.Goblin);
            CardList.Add("Goblin", new CardTemplate("Goblin", 20, Elements.Fire, Types.Monster, Factions.Goblin));
            CardList.Add("Elf", new CardTemplate("Elf", 20, Elements.Nature, Types.Monster, Factions.Elf));
            CardList.Add("WaterSpear", new CardTemplate("Water Spear", 20, Elements.Water, Types.Spell, Factions.NoFaction));
            CardList.Add("Kraken", new CardTemplate("Kraken", 20, Elements.Water, Types.Monster, Factions.Kraken));
            //Elf = new CardTemplate("Elf", 20, Elements.Nature, Types.Monster, Factions.Elf);
            //WaterSpear = new CardTemplate("Water Spear", 20, Elements.Water, Types.Spell, Factions.NoFaction);
            //Kraken = new CardTemplate("Kraken", 20, Elements.Water, Types.Monster, Factions.Kraken);
        }

        [TestCase("Elf", "Goblin", 1f)]
        [TestCase("Goblin", "WaterSpear", 0.5f)]
        [TestCase("WaterSpear", "Elf", 0.5f)]
        public void UnitTest_Compare_Elements(string left, string right, float expected)
        {
            float result = FightControler.ElementMultiplier(CardList[left], CardList[right]);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("WaterSpear", "Elf", 1.0f)]
        [TestCase("Goblin", "WaterSpear", 1.5f)]
        [TestCase("WaterSpear", "Kraken", 0.0f)]
        public void UnitTest_FactionCompare_Faction(string left, string right, float expected)
        {
            float result = FightControler.FactionMultiplier(CardList[left], CardList[right]);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}