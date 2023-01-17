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
    public class QueueHandlerUnitTest
    {
        User Player1;
        User Player2;
        Stats Player1Stats;
        Stats Player2Stats;

        [SetUp]
        public void Setup()
        {
            UserHandler.CreateUser("Player1", "Tests");
            UserHandler.CreateUser("Player2", "Tests");
            StatsAccess.UpdateStats(new Stats("Player1", 1000, 0, 0));
            StatsAccess.UpdateStats(new Stats("Player2", 0, 0, 0));

            QueueHandler.MoveElo(new("Player2", "Test", "Test", "Test", 5), new("Player1", "Test", "Test", "Test", 5));

            Player1 = UserAccess.GetUser("Player1");
            Player2 = UserAccess.GetUser("Player2");

            Player1Stats = StatsAccess.GetStats("Player1");
            Player2Stats = StatsAccess.GetStats("Player2");
        }

        [Test]
        public void TestEloMovement()
        {
            Assert.AreEqual(950, Player1Stats.Elo);
            Assert.AreEqual(50, Player2Stats.Elo);
        }

        [Test]
        public void TestEloMovementDifferential()
        {
            Assert.That(Player1Stats.Elo + Player2Stats.Elo == 1000);
        }

        [Test]
        public void TestWins()
        {
            Assert.That(Player1Stats.Wins == 0);
            Assert.That(Player2Stats.Wins == 1);
        }

        [Test]
        public void TestLosses()
        {
            Assert.That(Player1Stats.Losses == 1);
            Assert.That(Player2Stats.Losses == 0);
        }

        [Test]
        public void TestQueueing()
        {
            QueueEntry Entry = QueueAccess.EnterQueue(new User("Test1", "Test1", "Test1", "Test1", 0), new Deck("Test1"));
            Assert.That(Entry.Open == true && Entry.Finished == false);
            Entry = QueueAccess.EnterQueue(new User("Test2", "Test2", "Test2", "Test2", 0), new Deck("Test2"));
            Assert.That(Entry.Open == false && Entry.Finished == false);
        }
    }
}
