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
using System.Linq;

namespace Test
{
    [TestClass]
    public class TestTranslations
    {
        private readonly TranslationService _translationService;

        public TestTranslations()
        {
            var key = File.ReadAllText(@"C:\Projekt\secrets.txt");
            _translationService = new TranslationService(new AppSettings() { TranslateKey = key });

        } 

        [DataTestMethod]
        //    //Arrange
        [DataRow(new string[] {"hello", "world"})]
        [DataRow(new string[] {"HeLlO", "wOrLd"})]
        public void DiffrentCapitalizationReturnsCapitalizedWords(string[] inputList)
        {
            var expected = new List<string>() { "Hej", "Världen" };

         //    //Act
            var resp = _translationService.TranslateText(inputList.ToList()).Result;

        //    //Assert
            CollectionAssert.AreEqual(expected, resp);
        }

        // * Inga ord
        // * blanka tecken (space, tabb)
        // * null
        // * många ord 
        // * Skicka in "blaha" ord, dvs inte engelska
        
    }
}
