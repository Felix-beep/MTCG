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

        [SetUp]
        public void Setup()
        {
            Player1 = new User("Player1", "Test", "Test", "test", 20);
            Player2 = new User("Player2", "Test", "Test", "test", 20);
            Deck Deck1 = new Deck("Player1");
            Deck Deck2 = new Deck("Player1");
            QueueEntries = new QueueEntry(Player1, Deck1);
            QueueEntries.MutexFinished.ReleaseMutex();
            QueueEntries.Join(Player2, Deck2);
        }

        [Test]
        public void EmptyDeckEntry()
        {
            
            CombatHandler CH = new CombatHandler(QueueEntries);
            int Result = CH.StartCombat();
            Assert.That(Result == 2);

        }
    }
}
