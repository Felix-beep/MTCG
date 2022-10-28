using MTCG.BL;
using System.Diagnostics;

namespace MTCG.Test
{
    public class UnitTestsPackHandler
    {

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void UnitTest_PackHandler_CreateEmptyPack()
        {
            Pack newPack = PackHandler.GetEmptyPack(5);
            if (newPack.Packsize == 5 && newPack.MyList.Count == 0)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        public void UnitTest_PackHandler_CreateRandomPack_Size0()
        {
            Pack newPack = PackHandler.GetRandomPack();
            if (newPack.Packsize == 5 && newPack.MyList.Count == 5)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        public void UnitTest_PackHandler_CreateRandomPack_Size5()
        {
            Pack newPack = PackHandler.GetRandomPack(5);
            if (newPack.Packsize == 5 && newPack.MyList.Count == 5)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        public void UnitTest_PackHandler_CreateRandomPack_Size10()
        {
            Pack newPack = PackHandler.GetRandomPack(10);
            if (newPack.Packsize == 10 && newPack.MyList.Count == 10)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}