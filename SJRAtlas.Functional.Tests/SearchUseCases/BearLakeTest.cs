using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class BearLakeTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForBearLakeInSJRBasin()
        {
            Open("/home/index.rails");
			Selenium.Type("txt_search", "bear lake");
			Selenium.Click("btn_search");
            WaitForPageToLoad();
            AssertPlaceNameResults(11);

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAFUG')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Bear Lake, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataContains("Water Body Attributes");
        }

        [Test]
        public void TestQuickSearchForBearLakeWithFishStockingData()
        {
            Open("/home/index.rails");
			Selenium.Type("txt_search", "bear lake");
			Selenium.Click("btn_search");
            WaitForPageToLoad();
            AssertPlaceNameResults(11);

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAFUI')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Bear Lake, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataContains("Fish Stocking", "Water Body Attributes");
        }
	

        [Test]
        public void TestQuickSearchForBearLakeNotInSJRBasin()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "bear lake");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            Assert.AreEqual(11, Selenium.GetXpathCount("//div[@class=\"placename\"]"));

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAFUE')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Bear Lake, New Brunswick"));
            AssertInteractiveMapsIsEmpty();
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataIsEmpty();
        }	
    }
}
