using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InductionTest.MyData.UnitTests
{
    [TestClass]
    public class MyDataTests
    {
        [TestMethod]
        public void TestSortByMyInt()
        {
            var lst = new[]
            {
                new MyData { MyInt = 3 },
                new MyData { MyInt = 1 },
                null,
                new MyData { MyInt = 2 },
            }.ToList();
            lst.SortByMyInt();

            AssertMyDataList(lst, new[]
            {
                null,
                new MyData { MyInt = 1 },
                new MyData { MyInt = 2 },
                new MyData { MyInt = 3 },
            });
        }

        [TestMethod]
        public void TestSortByMyString()
        {
            var lst = new[]
            {
                new MyData { MyString = "c" },
                new MyData { MyString = null },
                new MyData { MyString = "bb" },
                null,
            }.ToList();
            lst.SortByMyString();

            AssertMyDataList(lst, new[]
            {
                null,
                new MyData { MyString = null },
                new MyData { MyString = "bb" },
                new MyData { MyString = "c" },
            });
        }

        void AssertMyDataList(List<MyData> actualMyDataList, MyData[] expectedMyDataObjects)
        {
            Assert.AreEqual(expectedMyDataObjects.Length, actualMyDataList.Count);

            for (int i = 0; i < actualMyDataList.Count; ++i)
            {
                var actualMyData = actualMyDataList[i];
                var expectedMyData = expectedMyDataObjects[i];

                if (actualMyData != null && expectedMyData != null)
                {
                    Assert.AreEqual(actualMyData.MyInt, expectedMyData.MyInt);
                    Assert.AreEqual(actualMyData.MyString, expectedMyData.MyString);
                }
                else
                {
                    Assert.IsTrue(actualMyData == null && expectedMyData == null);
                }
            }
        }
    }
}
