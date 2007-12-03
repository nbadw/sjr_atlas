namespace SJRAtlas.CGNS.Tests
{
    using System;
    using NUnit.Framework;
    using SJRAtlas.CGNS;
    using System.IO;
    using System.Reflection;

    [TestFixture]
    public class GnssPlaceNameTest
    {
        private string PLACENAME_XML = "SJRAtlas.CGNS.Tests.GnssPlaceName.xml";

        [Test]
        public void TestGnssPlaceNameToXml()
        {
            GnssPlaceName pn = new GnssPlaceName();
            pn.geoname = "Saint John";
            pn.location = "Saint John";
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

            Assert.AreEqual(ReadGnssPlaceNameXml(), pn.ToXml());
        }

        private string ReadGnssPlaceNameXml()
        {
            TextReader textReader = new StreamReader(getResourceStream(PLACENAME_XML));
            string expected = textReader.ReadToEnd().Trim();
            textReader.Close();
            return expected;
        }

        private Stream getResourceStream(string name)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        }

        [Test]
        public void TestGnssPlaceNameFromXml()
        {
            GnssPlaceName pn = GnssPlaceName.CreateFromXml(getResourceStream(PLACENAME_XML));
            Assert.AreEqual("Saint John", pn.geoname);
            Assert.AreEqual("Saint John", pn.location);
            Assert.AreEqual("Official", pn.status_term);
            Assert.AreEqual("45° 20' North", pn.latitude);
            Assert.AreEqual("65° 50' West", pn.longitude);
            Assert.AreEqual("45.3333000", pn.latdec);
            Assert.AreEqual("-65.8333000", pn.londec);
            Assert.AreEqual("2000", pn.coord_acc_m);
            Assert.AreEqual("Geographical area", pn.concise_term);
            Assert.AreEqual("County (2)", pn.generic_term);
            Assert.AreEqual("New Brunswick", pn.region_name);
            Assert.AreEqual("021H05,021G01,021G08,021H06,021H11", pn.nts_map);
            Assert.AreEqual("NAD83", pn.datum);
            Assert.AreEqual("DBBQU", pn.cgndb_key);
            Assert.AreEqual("0c82799c849c20c3418587608430d03d", pn.feature_id);
            Assert.AreEqual("GEOG", pn.concise_type);
        }
    }
}
