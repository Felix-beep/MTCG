using MTCG.BL;
using MTCG.BL.CombatHandling;
using MTCG.DatabaseAccess;
using MTCG.DatabaseAccess.DatabaseAccessers;
using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Test.UnitTests.Combat
{
    public class CombatMechanicsUnitTests
    {
        QueueEntry QueueEntries;
        User Player1;
        User Player2;
        CardInstance Instance1;
        CardInstance Instance2;
        CardTemplate Template1;
        CardTemplate Template2;

        [SetUp]
        public void Setup()
        {
            Player1 = new User("Player1", "Test", "Test", "test", 20);
            Player2 = new User("Player2", "Test", "Test", "test", 20);
            Deck Deck1 = new Deck("Player1");
            Deck Deck2 = new Deck("Player1");
            Template1 = new("TestSpell", 20, "Fire", "Spell", "NoFaction");
            Template2 = new("TestMonster", 20, "Water", "Monster", "NoFaction");
            Instance1 = new(0, "TestSpell", "123", Template1);
            Instance2 = new(0, "TestMonster", "123", Template2);
        }

        [Test]
        public void TestFactionsAttacker()
        {
            double result = FightCalculations.FactionMultiplier(Template2, Template1);
            Assert.That(result == 1);
        }

        [Test]
        public void TestFactionsDefender()
        {
            double result = FightCalculations.FactionMultiplier(Template1, Template2);
            Assert.That(result == 1);
        }

        [Test]
        public void TestElementsAttacker()
        {
            double result = FightCalculations.ElementMultiplier(Template1, Template2);
            Assert.That(result == 0.5);
        }
        [Test]
        public void TestElementsDefender()
        {
            double result = FightCalculations.ElementMultiplier(Template2, Template1);
            Assert.That(result == 2);
        }

        [Test]
        public void TestStringConstruction()
        {
            string TestString = FightCalculations.fillStringWithSpaces("ABC", 5);
            Assert.AreEqual("ABC  ", TestString);
        }

        [Test]
        public void TestBiggestNumberEqual()
        {
            int biggestNumber = FightCalculations.GetBiggestNumber(1,1,1);
            Assert.AreEqual(1, biggestNumber);
        }

        [Test]
        public void TestBiggestNumberA()
        {
            int biggestNumber = FightCalculations.GetBiggestNumber(3, 2, 1);
            Assert.AreEqual(3, biggestNumber);
        }

        [Test]
        public void TestBiggestNumberB()
        {
            int biggestNumber = FightCalculations.GetBiggestNumber(2, 3, 2);
            Assert.AreEqual(3, biggestNumber);
        }

        [Test]
        public void TestBiggestNumberC()
        {
            int biggestNumber = FightCalculations.GetBiggestNumber(2, 3, 1000);
            Assert.AreEqual(1000, biggestNumber);
        }

        [Test]
        public void CompareCards()
        {
            int Winner = FightCalculations.Compare(Instance1, Instance2);
            Assert.That(Winner == 2);
        }

        [Test]
        public void CompareCardsDraw()
        {
            int Winner = FightCalculations.Compare(Instance1, Instance1);
            Assert.That(Winner == 0);
        }
    }
}
