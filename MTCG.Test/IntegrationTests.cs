using MTCG.BL;
using MTCG.Models;
using MTCG.MODELS;
using MTCG.DatabaseAccess;
using MTCG.Database;
using MTCG.DatabaseAccess.DatabaseAccessers;

namespace MTCG.Test
{
    public class IntegrationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IntegrationTest_CreateCards_CreateUser_CreateDecks_PerformCombat()
        {
            UserAccess.CreateUser("Felix", "Kainz");
            User Felix = UserAccess.GetUser("Felix");
            Assert.That(Felix.Name == "Felix");
        }
    }
}