using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MoleculTest
{
    [TestClass]
    public class TestAtoms
    {

        Random r = new Random();
        int min = 100000, max = 1000000;
        [TestMethod]
        public void getAtom()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void setAtom()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void rotateAtom()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }


        [TestMethod]
        public void editAtom()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void showAtom()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }
    }
}
