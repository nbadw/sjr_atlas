using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class CampbellRiverTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForCampbellRiver()
        {
            Open("/home/index.rails");
			Selenium.Type("txt_search", "campbell river");
			Selenium.Click("btn_search");
            WaitForPageToLoad();

            Assert.AreEqual(3, Selenium.GetXpathCount("//div[@class=\"placename\"]"));
            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAWFZ')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Campbell River"));
            AssertInteractiveMapsIsEmpty();
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataIsEmpty();
        }
	
    }
}
