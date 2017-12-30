using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntelliCG.Pet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Pet.Tests
{
    [TestClass()]
    public class PetsTests
    {
        [TestMethod()]
        public void PetsTest()
        {
            var map=new Hashtable();
            map["asd"] = "asd";
            var a=map["as1d"];
            Assert.IsNotNull(a);
        }
    }
}