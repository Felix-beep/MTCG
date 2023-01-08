using MTCG.BL;
using MTCG.Models;

namespace MTCG.Test
{
    public class UnitTestsBLUser
    {
        private User User1;

        [SetUp]
        public void Setup()
        {
            User1 = new User(1, "Ralf");
        }

        [Test]
        public void UserCreationTest_Name()
        {
            if (User1.Name != "Ralf") Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void UserCreationTest_ID()
        {
            if (User1.Id != 1) Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void UserCreationTest_StackSize()
        {
            if (User1.CardList.Size != 0) Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void UserCreationTest_DeckSize()
        {
            if (User1.DeckList.Size != 0) Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void UserCreationTest_Elo()
        {
            if (User1.Elo != 1000) Assert.Fail();
            Assert.Pass();
        }
    }
}