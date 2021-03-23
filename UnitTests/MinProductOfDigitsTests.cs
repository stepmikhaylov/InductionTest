using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InductionTest.MinProductOfDigits.UnitTests
{
    [TestClass]
    public class MinProductOfDigitsTests
    {
        [TestMethod]
        public void TestFindMinProductOfDigits()
        {
            Assert.AreEqual(25, Program.FindMinProductOfDigits(10));
            Assert.AreEqual(8, Program.FindMinProductOfDigits(8));
            Assert.AreEqual(-1, Program.FindMinProductOfDigits(13));
            Assert.AreEqual(259, Program.FindMinProductOfDigits(90));
            Assert.AreEqual(-1, Program.FindMinProductOfDigits(22));
            Assert.AreEqual(67, Program.FindMinProductOfDigits(42));
        }
    }
}
