using System.Diagnostics;

namespace MTCG.Test
{
    public class UnitTestsPacks
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UnitTest_CreateEmptyPack()
        {
            Pack newPack = new Pack(5);
            if (newPack.Packsize == 5 && newPack.MyList.Count == 0)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}