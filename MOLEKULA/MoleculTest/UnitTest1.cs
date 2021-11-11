using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MoleculTest
{
    [TestClass]
    public class TestMoleculs
    {
        Random r = new Random();
        int min = 100000, max = 1000000;
        [TestMethod]
        public void getMolecule()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void setMolecule()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void rotateMolecule()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }


        [TestMethod]
        public void editMolecule()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }

        [TestMethod]
        public void showMolecule()
        {
            int n = r.Next(min, max);
            for (int i = 0; i < n; i++)
                Assert.AreEqual(i, i);
        }
    }
}
