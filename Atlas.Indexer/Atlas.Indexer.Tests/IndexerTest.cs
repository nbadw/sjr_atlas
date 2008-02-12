using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Atlas.Indexer;
using System.IO;
using SJRAtlas.Models;
using log4net;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord;
using SJRAtlas.Models.Atlas;

[TestFixture]
public class IndexerTest
{
    private string testDataDirectory;
    private string metadataDirectory;
    private Indexer indexer;

    [TestFixtureSetUp]
    public void Initialize()
    {
        testDataDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\TestData");
        metadataDirectory = Path.Combine(testDataDirectory, "Metadata");
        indexer = new Indexer(metadataDirectory);
    }

    [TestFixtureTearDown]
    public void Teardown()
    {
        ActiveRecordStarter.DropSchema();
    }

    [Test]
    public void TestIndex()
    {
        Assert.Fail();
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestIndexWhenFileIsNull()
    {
        indexer.Index(null);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestIndexWhenFileDoesNotExist()
    {
        indexer.Index(@"c:\path\to\non-existant\file");
    }

    [Test]
    [ExpectedException(typeof(Exception))]
    public void TestIndexWhenFileIsOutsideMetadataDirectory()
    {
        string file = Path.Combine(testDataDirectory, @"NotMetadataDirectory\file-outside-metadata-directory-root.xml");
        indexer.Index(file);
    }

    [Test]
    [ExpectedException(typeof(Exception))]
    public void TestIndexWhenFileDirectoryIsMetadataDirectory()
    {
        string file = Path.Combine(metadataDirectory, "file-at-metadata-directory-root.xml");
        indexer.Index(file);
    }

    [Test]
    public void TestBuildItemToIndex()
    {
        Assert.Fail();
    }

    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "No IMetadataAware classes could be found to match the type named NonExistantType")]
    public void TestIndexWhenTypeCannotBeMatched()
    {
        string file = Path.Combine(metadataDirectory, @"NonExistantType\testfile.xml");
        indexer.Index(file);
    }

    [Test]
    [ExpectedException(typeof(System.Xml.XmlException))]
    public void TestIndexWhenFileIsNotActuallyXml()
    {
        string file = Path.Combine(metadataDirectory, @"PublishedReports\not-actually-xml.xml");
        indexer.Index(file);
    }

    [Test]
    [ExpectedException(typeof(Exception))]
    public void TestIndexWhenXmlFileDoesNotHaveMetadataAsRootElement()
    {
        string file = Path.Combine(metadataDirectory, @"PublishedReports\root-element-not-metadata.xml");
        indexer.Index(file);
    }
}
