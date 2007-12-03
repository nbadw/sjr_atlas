namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    using NUnit.Framework;

    [TestFixture]
    public class MonctonTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForMoncton()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "moncton");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            Assert.AreEqual(6, Selenium.GetXpathCount("//div[@class=\"placename\"]"));

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DADHJ')]");
            WaitForPageToLoad();
            
            Assert.IsTrue(Selenium.IsTextPresent("Moncton, New Brunswick"));
            AssertInteractiveMapsIsEmpty();
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataIsEmpty();        
        }
	
    }
}
