using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class HammondRiverTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForHammondRiverAndUserSelectsTheRiver()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "hammond river");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            Assert.AreEqual(2, Selenium.GetXpathCount("//div[@class=\"placename\"]"));

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAJFO')]");
            WaitForPageToLoad();
            
            Assert.IsTrue(Selenium.IsTextPresent("Hammond River, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataContains("Electrofishing", "Fish Counts", "Fish Stocking",
                "Sportfishing – Atlantic Salmon", "Stream Survey", "Water Bacteria Sampling",
                "Water Body Attributes", "Water Chemistry Sampling", "Water Temperature Monitoring",
                "Watershed Unit");
        }

        public void TestQuickSearchForHammondRiverAndUserSelectsTheRailwayPoint()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "hammond river");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            Assert.AreEqual(2, Selenium.GetXpathCount("//div[@class=\"placename\"]"));

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAJFN')]");
            WaitForPageToLoad();
            
            Assert.IsTrue(Selenium.IsTextPresent("Hammond River, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertMetadataSectionIsEmpty("datasets");
        }
    }
}
