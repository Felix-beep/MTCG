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

        [OneTimeSetUp]
        public void Setup()
        {
            Player1 = new User("Player1", "Test", "Test", "test", 20);
            Player1Stats = new Stats("Player1", 1000, 0, 0);
            Player2 = new User("Player2", "Test", "Test", "test", 20);
            Player2Stats = new Stats("Player2", 0, 0, 0);
        }

        [Test]
        public void TestEloMovement()
        {
            QueueHandler.MoveElo(Player1, Player2);

            Assert.That(Player1Stats.Elo == 900 && Player1Stats.Elo == 100);
        }

        [Test]
        public void TestEloMovementDifferential()
        {
            QueueHandler.MoveElo(Player1, Player2);

            Assert.That(Player1Stats.Elo + Player1Stats.Elo == 1000);
        }

        [Test]
        public void TestWins()
        {
            Assert.That(Player1Stats.Wins == 2);
            Assert.That(Player2Stats.Wins == 0);
        }

        [Test]
        public void TestLosses()
        {
            Assert.That(Player1Stats.Losses == 0);
            Assert.That(Player2Stats.Losses == 2);
        }

        [Test]
        public void TestQueueing()
        {
            QueueEntry Entry = QueueAccess.EnterQueue(new User("Test1", "Test1", "Test1", "Test1", 0), new Deck("Test1"));
            Assert.That(Entry.Open == true && Entry.Finished == false);
        }

        [Test]
        public void TestJoiningQueueing()
        {
            QueueEntry Entry = QueueAccess.EnterQueue(new User("Test2", "Test2", "Test2", "Test2", 0), new Deck("Test2"));
            Assert.That(Entry.Open == false && Entry.Finished == false);
        }
    }
}
