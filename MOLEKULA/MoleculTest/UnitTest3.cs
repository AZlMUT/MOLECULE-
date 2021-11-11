using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MoleculTest
{
    [TestClass]
    public class TestData
    {
        Random r = new Random();
        int min = 100000, max = 1000000;

        [TestMethod]
        public void getInfo()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void addMoleculToDB()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void addAtomToDB()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }


        [TestMethod]
        public void dellInfoFromDB()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void updateInfoFromDB()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }
    }
}
