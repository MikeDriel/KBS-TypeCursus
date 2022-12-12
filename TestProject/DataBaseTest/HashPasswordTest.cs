using Controller;
using Model;
using System;

namespace TestProject.DataBaseTest
{
    [TestFixture]
    public class Tests
    {

        private string _woord1;
        private string _woord2;
        private string _woord1hashed;
        private string _woord2hashed;
        private Database _db;



        [SetUp]
        public void Setup()
        {
            _woord1 = "test";
            _db = new Database();
            _woord1hashed = _db.HashPassword(_woord1);
        }

        [Test]
        public void Test1()
        {
            _woord2 = "test";
            _woord2hashed = _db.HashPassword(_woord2);
            Assert.AreEqual(_woord1hashed, _woord2hashed);
        }

        [Test]
        public void Test2()
        {
            _woord2 = "incorrect";
            _woord2hashed = _db.HashPassword(_woord2);
            Assert.AreNotEqual(_woord1hashed, _woord2hashed);
        }


    }
}