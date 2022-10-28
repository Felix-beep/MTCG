using MTCG.BL;
using MTCG.Models;

namespace MTCG.Test
{
    public class UnitTestsCombatCards
    {
        CardInstance Goblin;
        CardInstance Elf;
        CardInstance WaterSpear;
        CardInstance Kraken;

        [SetUp]
        public void Setup()
        {
            Goblin = new CardInstance(new CardTemplate("Goblin", 20, Elements.Fire, Types.Monster, Factions.Goblin), 0);
            Elf = new CardInstance(new CardTemplate("Elf", 20, Elements.Nature, Types.Monster, Factions.Elf), 0);
            WaterSpear = new CardInstance(new CardTemplate("Water Spear", 20, Elements.Water, Types.Spell, Factions.NoFaction), 0);
            Kraken = new CardInstance(new CardTemplate("Kraken", 20, Elements.Water, Types.Monster, Factions.Kraken), 0);
        }

        [Test]
        public void UnitTest_CardCompare_GoblinElf()
        {
            int result = FightControler.Compare(Goblin, Elf);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void UnitTest_CardCompare_GoblinWaterSpear()
        {
            int result = FightControler.Compare(Goblin, WaterSpear);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void UnitTest_CardCompare_KrakenWaterSpear()
        {
            int result = FightControler.Compare(Kraken, WaterSpear);
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void UnitTest_CardCompare_WaterSpearElf()
        {
            int result = FightControler.Compare(WaterSpear, Elf);
            Assert.That(result, Is.EqualTo(1));
        }

    }
}