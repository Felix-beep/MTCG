using MTCG.BL;
using MTCG.Models;
using MTCG.MODELS;

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
            CardTemplate myCard1 = new CardTemplate("Goblin", 20, Elements.Fire, Types.Monster, Factions.Goblin);
            CardTemplate myCard2 = new CardTemplate("Elf", 20, Elements.Nature, Types.Monster, Factions.Elf);

            User firstUser = new User(1, "Rupert");
            for (int i = 0; i < 4; i++)
            {
                CardInstance newCard = new CardInstance(myCard1);
                firstUser.AddCard(newCard);
                firstUser.AddCardToDeck(newCard);
            }

            User secondUser = new User(2, "Stefan");
            for (int i = 0; i < 4; i++)
            {
                CardInstance newCard = new CardInstance(myCard2);
                secondUser.AddCard(newCard);
                secondUser.AddCardToDeck(newCard);
            }

            firstUser.CardList.Print();

            CombatHandler myCombat = new CombatHandler();

            myCombat.AddPlayer(firstUser);

            myCombat.AddPlayer(secondUser);

            myCombat.StartCombat();

            Assert.Pass();

        }
    }
}