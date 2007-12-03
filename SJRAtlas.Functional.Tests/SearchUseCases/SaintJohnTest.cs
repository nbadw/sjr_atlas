namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class SaintJohnTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForSaintJohnAndUserSelectsTheCity()
        {
			Open("/home/index.rails");
			Selenium.Type("txt_search", "saint john");
			Selenium.Click("btn_search");
            WaitForPageToLoad();
            Assert.AreEqual(7, Selenium.GetXpathCount("//div[@class=\"placename\"]"));

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAEGW')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Saint John, New Brunswick"));
            AssertInteractiveMapsContains("City of Saint John Map", "Topographic Map",
                "Major Landowners Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsContains("City of Saint John Green Policy");
            AssertDataIsEmpty();
        }

        [Test]
        public void TestQuickSearchForSaintJohnAndUserDoesNotSelectTheCity()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "saint john");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            Assert.AreEqual(7, Selenium.GetXpathCount("//div[@class=\"placename\"]"));

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAEEI')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Saint John Harbour, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataIsEmpty();
        }
	
	
    }
}
