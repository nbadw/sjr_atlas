using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class ArnoldBrookTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForArnoldBrookAndUserSelectsTheResultNotInTheBasin()
        {
            Open("/home/index.rails");
			Selenium.Type("txt_search", "arnold brook");
			Selenium.Click("btn_search");
            WaitForPageToLoad();
            AssertPlaceNameResults(3);

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAUTK')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Arnold Brook, New Brunswick"));
            AssertInteractiveMapsIsEmpty();
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataIsEmpty();
        }

        [Test]
        public void TestQuickSearchForArnoldBrookAndUserSelectsTheResultInTheBasin()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "arnold brook");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            AssertPlaceNameResults(3); ;

            Selenium.Click("//a[contains(@href, '/placename/view.rails?id=DAUTJ')]");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Arnold Brook, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataContains("Fish Stocking", "Water Body Attributes");
        }	
    }
}
