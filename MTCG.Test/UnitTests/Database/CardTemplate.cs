using MTCG.BL;
using MTCG.Models;
using MTCG.MODELS;
using MTCG.DatabaseAccess;
using MTCG.Database;
using MTCG.DatabaseAccess.DatabaseAccessers;
using System.Xml.Serialization;


namespace MTCG.Test.UnitTests.Database
{
    public class CardTemplateUnitTests
    {
        public CardTemplate CardTemplateIn = new("FireGoblin", 20, "Fire", "Monster", "Goblin");
        public CardTemplate CardTemplateOut;

        [OneTimeSetUp]
        public void Setup()
        {
            CardTemplateAccess.CreateCardTemplate(CardTemplateIn.Name, CardTemplateIn.Power, CardTemplateIn.Element.ToString(), CardTemplateIn.Type.ToString(), CardTemplateIn.Faction.ToString());
            CardTemplateOut = CardTemplateAccess.GetCardTemplate(CardTemplateIn.Name);
        }

        [Test]
        public void Name()
        {
            Console.WriteLine(CardTemplateOut.Name);
            Assert.That(CardTemplateOut.Name == CardTemplateOut.Name);
        }

        [Test]
        public void Power()
        {
            Console.WriteLine(CardTemplateOut.Power);
            Assert.That(CardTemplateOut.Power == CardTemplateIn.Power);
        }

        [Test]
        public void Faction()
        {
            Assert.That(CardTemplateOut.Faction == CardTemplateIn.Faction);
        }

        [Test]
        public void Type()
        {
            Assert.That(CardTemplateOut.Type == CardTemplateIn.Type);
        }

        [Test]
        public void Element()
        {
            Assert.That(CardTemplateOut.Element == CardTemplateIn.Element);
        }

        [Test]
        public void Delete()
        {
            CardTemplateAccess.DeleteAllCardTemplaes();
            Assert.That(CardTemplateAccess.GetCardTemplate(CardTemplateIn.Name) == null);
        }
    }
}