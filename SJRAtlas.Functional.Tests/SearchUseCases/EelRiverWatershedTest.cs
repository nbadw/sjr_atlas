using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class EelRiverWatershedTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForEelRiverAndUserSelectsTheResultInTheBasin()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "eel river watershed");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            Assert.AreEqual(3, Selenium.GetXpathCount("//tr[@class='watershed']"));

            Selenium.Click("//a[contains(@href, '/watershed/view.rails?id=01-26-00-00-00-00')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Eel River, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map", "Lake Depths Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataContains("Fish Stocking", "Sportfishing – Atlantic Salmon",
                "Water Body Attributes", "Watershed Unit");

        }

        [Test]
        public void TestQuickSearchForEelRiverAndUserSelectsTheResultNotInTheBasin()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "eel river watershed");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            Assert.AreEqual(3, Selenium.GetXpathCount("//tr[@class='watershed']"));

            Selenium.Click("//a[contains(@href, '/watershed/view.rails?id=02-01-02-00-00-00')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Eel River, New Brunswick"));
            AssertInteractiveMapsIsEmpty();
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataIsEmpty();
        }

    }
}
