using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests.SearchUseCases
{
    [TestFixture]
    public class OromoctoLakeTest : BaseSeleniumTest
    {
        [Test]
        public void TestQuickSearchForOromoctoLake()
        {
            Open("/home/index.rails");
            Selenium.Type("txt_search", "oromocto lake");
            Selenium.Click("btn_search");
            WaitForPageToLoad();
            
            Assert.IsTrue(Selenium.IsTextPresent("Oromocto Lake, New Brunswick"));
            AssertInteractiveMapsContains("Major Landowners Map", "Topographic Map", "Lake Depths");
            AssertPublishedMapsIsEmpty();
            AssertReportsIsEmpty();
            AssertDataContains("Fish Stocking", "Lake Survey",
                "Water Body Attributes");
        }
	
    }
}
