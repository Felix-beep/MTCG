using MTCG.BL;
using MTCG.MODELS;
using System.Diagnostics;

namespace MTCG.Test
{
    public class UnitTestsBLDeck
    {
        private Deck myDeck;
        private CardTemplate myCardTemplate;
        private CardInstance myCardInstance;

        [SetUp]
        public void Setup()
        {
            myDeck = new Deck();
            myCardTemplate = new CardTemplate();
            myCardInstance = new CardInstance(myCardTemplate);

            myDeck.AddCard(myCardInstance);
            myDeck.AddCard(myCardInstance);
        }

        [Test]
        public void DeckTest_Creation()
        {
            if (myDeck.Size != 2) Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void DeckTest_Add()
        {
            myDeck.AddCard(myCardInstance);
            if (myDeck.Size != 3) Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void DeckTest_GetCard()
        {
            if (myDeck.GetRandomCard().BaseCard.Name != myCardTemplate.Name) Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void UserCreationTest_PopCard_GetCard()
        {
            if (myDeck.PopRandomCard().BaseCard.Name != myCardTemplate.Name) Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void UserCreationTest_PopCard_Size()
        {
            myDeck.PopRandomCard();
            if (myDeck.Size != 1) Assert.Fail();
            Assert.Pass();
        }
    }
}