using MTCG.BL;
using MTCG.Models;
using MTCG.MODELS;
using MTCG.DatabaseAccess;
using MTCG.Database;
using MTCG.DatabaseAccess.DatabaseAccessers;
using System.Xml.Serialization;


namespace MTCG.Test.UnitTests.Database
{
    public class UserUnitTests
    {
        public User UserIn = new ("Elu", "Kainz", "Custom Bio", "Custom Picture", 20);
        public User UserOut;

        [OneTimeSetUp]
        public void Setup()
        {
            UserAccess.CreateUser(UserIn.Name, UserIn.Password);
            UserAccess.EditUser(UserIn.Name, UserIn.Bio, UserIn.Image);
            UserOut = UserAccess.GetUser(UserIn.Name);
        }

        [Test]
        public void Name()
        {
            Assert.That(UserIn.Name == UserOut.Name);
        }

        [Test]
        public void Password()
        {
            Assert.That(UserIn.Password == UserOut.Password);
        }

        [Test]
        public void Bio()
        {
            Assert.That(UserIn.Bio == UserOut.Bio);
        }

        [Test]
        public void Picture()
        {
            Assert.That(UserIn.Image == UserOut.Image);
        }

        [Test]
        public void Gold()
        {
            Assert.That(UserIn.Gold == UserOut.Gold);
        }

        [Test]
        public void Delete()
        {
            UserAccess.DeleteAllUsers();
            Assert.That(UserAccess.GetUser(UserIn.Name) == null);
        }
    }
}