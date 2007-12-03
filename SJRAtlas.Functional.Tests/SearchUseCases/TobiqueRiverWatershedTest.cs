using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class TobiqueRiverWatershedTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForTobiqueRiverWatershed()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "tobique river watershed");
            Selenium.Click("btn_search");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Tobique River, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataContains("Fish Count", "Fish Measurements", "Fish Stocking",
                "Lake Survey", "Sportfishing – Atlantic Salmon", "Stream Survey", "Water Bacteria Sampling",
                "Water Body Attributes", "Water Chemistry Sampling", "Water Temperature Monitoring",
                "Watershed Unit");
        }
	
    }
}
