using System;
using NUnit.Framework;
using WordSim;

namespace WordSimTest
{
    [TestFixture]
    public class StringMetricsTest
    {
        private delegate int StringMetric(string w1, string w2);

        private static readonly StringMetric Distance = StringMetrics.LevenshteinDistance;

        [SetUp]
        public void SetUp()
        {
            StringMetrics.Log = true;
        }
        
        [Test]
        public void TestSubstitute()
        {
            Assert.True(Distance("klaver", "claves") == 2);
        }

        [Test]
        public void TestDelete()
        {
            Assert.True(Distance("vend", "ven") == 1);
        }
        
        [Test]
        public void TestInsert()
        {
            Assert.True(Distance("ven", "vend") == 1);
        }
        
        [Test]
        public void TestSubIns()
        {
            Assert.True(Distance("hestesko", "bedstemor") == 5);
        }
        
        [Test]
        public void TestCommutative()
        {
            Assert.True(Distance("hest", "vej") == Distance("vej", "hest"));
        }
    }
}