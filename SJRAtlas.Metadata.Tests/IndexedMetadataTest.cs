using System;
using NUnit.Framework;
using System.IO;
using Rhino.Mocks;
using Lucene.Net.Documents;

namespace SJRAtlas.Metadata.Tests
{
    [TestFixture]
    public class IndexedMetadataTest
    {
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
        }

        [TearDown]
        public void Teardown()
        {
            mocks.VerifyAll();
        }

        [Test]
        public void TestToPdf()
        {
            //string expected_pdf = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\fgdc-metadata.pdf"));
            //string input_file = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\fgdc-metadata.xml"));
            //string output_file = Path.GetTempFileName();
            
            //Document doc = new Document();
            //doc.Add(new Field("_key", input_file, Field.Store.YES, Field.Index.UN_TOKENIZED));

            //Stream fout = new FileStream(output_file, FileMode.Open);

            //IndexedMetadata imetadata = new IndexedMetadata(1, doc);
            //imetadata.ToXml(fout);
            //fout.Close();

            //FileAssert.AreEqual(expected_pdf, output_file);
            //File.Delete(output_file);       
            Assert.Fail();
        }

        [Test]
        public void TestToXml()
        {
            //string input_file = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\fgdc-metadata.xml"));
            //string output_file = Path.GetTempFileName();

            //Document doc = new Document();
            //doc.Add(new Field("_key", input_file, Field.Store.YES, Field.Index.UN_TOKENIZED));

            //Stream fout = new FileStream(output_file, FileMode.Open);

            //IndexedMetadata imetadata = new IndexedMetadata(1, doc);
            //imetadata.ToXml(fout);
            //fout.Close();

            //FileAssert.AreEqual(input_file, output_file);
            //File.Delete(output_file);     
            Assert.Fail();
        }

        [Test]
        public void TestTitleFieldNotPresent()
        {
            // test code goes here
            Assert.Fail();
        }

        [Test]
        public void TestOriginFieldNotPresent()
        {
            // test code goes here
            Assert.Fail();
        }

        [Test]
        public void TestTimePeriodFieldNotPresent()
        {
            // test code goes here
            Assert.Fail();
        }

        [Test]
        public void TestAbstractFieldNotPresent()
        {
            // test code goes here
            Assert.Fail();
        }
	
    }
}
