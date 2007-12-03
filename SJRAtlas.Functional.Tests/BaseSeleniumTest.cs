namespace SJRAtlas.Functional.Tests
{
    using System;
    using NUnit.Framework;
    using Selenium;

    public abstract class BaseSeleniumTest
    {
        private ISelenium selenium;
        private string root;

        [TestFixtureSetUp]
        public void InitializeSelenium()
        {
            string host = "http://localhost:44051";
            root = "";
            //string host = "http://76.74.154.14";
            //root = "/site";
            selenium = new DefaultSelenium("localhost", 4444, "*firefox", host);
            selenium.Start();
        }

        [TestFixtureTearDown]
        public void TerminateSelenium()
        {
            try
            {
                selenium.Stop();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        protected ISelenium Selenium
        {
            get { return selenium; }
            set { selenium = value; }
        }

        protected void Open(string path)
        {
            Selenium.Open(root + path);
        }

        protected void AssertPlaceNameResults(int count)
        {
            Assert.AreEqual(count, Selenium.GetXpathCount("//div[@class=\"placename\"]"));
        }

        protected void AssertMetadataSectionContains(string divName, string[] titles)
        {
            Assert.AreEqual(titles.Length, Selenium.GetXpathCount("//div[@id=\"" + 
                divName + "\"]/ul/li"), "Section=" + divName + " does not contain " + titles.Length + " items");
            foreach (string title in titles)
            {
                Assert.IsTrue(Selenium.IsTextPresent(title), "Section=" + divName + " does not contain the text \"" + title + "\"");
            }
        }

        protected void AssertMetadataSectionIsEmpty(string section)
        {
            AssertMetadataSectionIsEmpty(section, section.Substring(0, 1).ToUpper() + 
                section.Substring(1));
        }

        protected void AssertMetadataSectionIsEmpty(string divName, string category)
        {
            Assert.AreEqual(0, Selenium.GetXpathCount("//div[@id=\"" + divName + "\"]/ul/li"), "Section=" + divName + " has more than 0 items");
            Assert.IsTrue(Selenium.IsTextPresent("No " + category + " Found"), "Section=" + divName + " does not contain the text \"No " + category + " Found\"");
        }

        protected void AssertPublishedMapsContains(params string[] titles)
        {
            AssertMetadataSectionContains("published_maps", titles);
        }

        protected void AssertPublishedMapsIsEmpty()
        {
            AssertMetadataSectionIsEmpty("published_maps", "Published Maps");
        }

        protected void AssertInteractiveMapsContains(params string[] titles)
        {
            AssertMetadataSectionContains("interactive_maps", titles);
        }

        protected void AssertInteractiveMapsIsEmpty()
        {
            AssertMetadataSectionIsEmpty("interactive_maps", "Interactive Maps");
        }

        protected void AssertReportsContains(params string[] titles)
        {
            AssertMetadataSectionContains("reports", titles);
        }

        protected void AssertReportsIsEmpty()
        {
            AssertMetadataSectionIsEmpty("reports");
        }

        protected void AssertDataContains(params string[] titles)
        {
            AssertMetadataSectionContains("datasets", titles);
        }

        protected void AssertDataIsEmpty()
        {
            AssertMetadataSectionIsEmpty("datasets", "Data");
        }

        protected void WaitForPageToLoad()
        {
            Selenium.WaitForPageToLoad(int.MaxValue.ToString());
        }
    }
}
