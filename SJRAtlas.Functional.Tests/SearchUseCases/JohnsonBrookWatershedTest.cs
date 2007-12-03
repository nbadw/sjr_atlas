using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class JohnsonBrookWatershedTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForJohnsonBrookWatershed()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "arnold brook watershed");
            Selenium.Click("btn_search");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Sorry, No Watersheds Found."), "'No Watersheds Found' message was not present");
        }
	
    }
}
