namespace SJRAtlas.CGNS.Tests
{
    using System;
    using NUnit.Framework;
    using SJRAtlas.CGNS;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;
    using System.Text;

    [TestFixture]
    public class GnssSearchResultsTest
    {
        private string RESULTS_XML = "SJRAtlas.CGNS.Tests.GnssSearchResults.xml";

        private string ReadGnssSearchResultsXml()
        {
            TextReader textReader = new StreamReader(getResourceStream(RESULTS_XML));
            string expected = textReader.ReadToEnd().Trim();
            textReader.Close();
            return expected;
        }

        private Stream getResourceStream(string name)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        }

        [Test]
        public void TestGnssSearchResultsToXml()
        {
            GnssSearchResults results = new GnssSearchResults();

            GnssPlaceName pn = new GnssPlaceName();
            pn.geoname = "Saint John";
            pn.status_term = "Official";
            pn.latitude = "45° 20' North";
            pn.longitude = "65° 50' West";
            pn.latdec = "45.3333000";
            pn.londec = "-65.8333000";
            pn.coord_acc_m = "2000";
            pn.concise_term = "Geographical area";
            pn.generic_term = "County (2)";
            pn.region_name = "New Brunswick";
            pn.nts_map = "021H05,021G01,021G08,021H06,021H11";
            pn.datum = "NAD83";
            pn.cgndb_key = "DBBQU";
            pn.feature_id = "0c82799c849c20c3418587608430d03d";
            pn.concise_type = "GEOG";

            results.Items = new GnssPlaceName[] { pn };

            Assert.AreEqual(ReadGnssSearchResultsXml(), results.ToXml());
        }

        [Test]
        public void TestGnssSearchResultsFromXml()
        {
            GnssSearchResults results = GnssSearchResults.CreateFromXml(getResourceStream(RESULTS_XML));
            Assert.AreEqual(1, results.Items.Length);
        }
    }
}
