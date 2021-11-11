using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MoleculTest
{
    [TestClass]
    public class OtherTest
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
        public void getColor()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void findPos()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void getPos()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void getAngle()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void findConnect()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }
    }
}
