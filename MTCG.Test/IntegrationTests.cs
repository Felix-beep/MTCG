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
            UserAccess.CreateUser("Elu", "Kainz");
            User Elu = UserAccess.GetUser("Elu");
            Assert.That(Elu.Name == "Elu");
        }
    }
}