using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snylta;
using Snylta.Data;
using Snylta.Models;
using Snylta.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace Test
{
    [TestClass]
    public class TestTranslations
    {
        //private readonly ThingsController _thingsController;

        //public ThingsController_IsAnImage()
        //{
        //    //_thingsController = new ThingsController();
        //} 

        [TestMethod]
        public void TestMethod1()
        {
            //var p =  Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //var key = System.IO.File.ReadAllText(p);

            var key = System.IO.File.ReadAllText(@"C:\Projekt\secrets.txt");
            var service = new TranslationService(new AppSettings() {TranslateKey = key});

            var inputList = new List<string>() { "hello", "world" };
            var expected = new List<string>() { "Hej", "Världen" };

            var resp = service.TranslateText(inputList).Result;
            CollectionAssert.AreEqual(expected, resp);
        }

        //[TestMethod]
        //public void TestMethod2()
        //{
        //    //Arrange

        //    //Act

        //    //Assert
        //}

        //[TestMethod]
        //public void TestMethod3()
        //{
        //    //Arrange

        //    //Act

        //    //Assert
        //}

        //[TestMethod]
        //public void TestMethod4()
        //{
        //    //Arrange

        //    //Act

        //    //Assert
        //}

        ////[TestMethod]
        ////public void TestMethod2()
        ////{
        ////    //Arrange

        ////    //Act

        ////    //Assert
        ////}

        ////[TestMethod]
        ////public void TestMethod2()
        ////{
        ////    //Arrange

        ////    //Act

        ////    //Assert
        ////}

        ////[TestMethod]
        ////public void TestMethod2()
        ////{
        ////    //Arrange

        ////    //Act

        ////    //Assert
        ////}
    }
}
