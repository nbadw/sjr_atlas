using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class SaintJohnRiverTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForSaintJohnRiver()
        {
            Open("/home/index.rails");
			Selenium.Type("txt_search", "saint john river");
			Selenium.Click("btn_search");
            WaitForPageToLoad();

            Assert.IsTrue(Selenium.IsTextPresent("Saint John River, New Brunswick"));

            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map");
            AssertPublishedMapsIsEmpty();            
            AssertReportsContains("Assessing non-point source pollution in agricultural regions of the upper St. John River basin using the slimy sculpin (Cottus cognatus)");
            AssertDataContains("Fish Counts", "Fish Stocking", "Sportfishing - Atlantic Salmon",
                "Water Body Attributes", "Water Temperature Monitoring", "Watershed Unit");
        }
	
    }
}
