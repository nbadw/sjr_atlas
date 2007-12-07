#region "Mandatory NUnit Imports"
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
#endregion

//Test Specific Imports
using Metadata.Indexer;
using System.IO;
using SJRAtlas.Models;
using log4net;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord;

[TestFixture]
public class IndexerTest
{
    private Indexer indexer;
    private string testDataDirectory;
    private string metadataDirectory;

    [TestFixtureSetUp]
    public void FixtureInit()
    {
        log4net.Config.XmlConfigurator.Configure();
        IConfigurationSource source = ActiveRecordSectionHandler.Instance;
        ActiveRecordStarter.Initialize(System.Reflection.Assembly.Load("SJRAtlas.Models"), source);
    }

    [SetUp]
    public void SetUp()
    {
        ActiveRecordStarter.CreateSchema();
        testDataDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\TestData");
        metadataDirectory = Path.Combine(testDataDirectory, "Metadata");

        indexer = new Indexer();
        indexer.MetadataDirectory = metadataDirectory;
        indexer.MetadataAwareTypes = new Type[] { typeof(PublishedReport) };
        indexer.Logger = LogManager.GetLogger(typeof(Indexer));
    }

    [TearDown]
    public void TearDown()
    {
        indexer = null;
        ActiveRecordStarter.DropSchema();
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
    [ExpectedException(typeof(Exception), ExpectedMessage="No IMetadataAware classes could be found to match the type named NonExistantType")]
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

    [Test]
    [ExpectedException(typeof(InvalidCastException))]
    public void TestIndexWhenMatchedTypeIsNotIMetadataAware()
    {
        indexer.MetadataAwareTypes = new Type[] { typeof(NotIMetadataAware) };
        string file = Path.Combine(metadataDirectory, @"NotIMetadataAware\testfile.xml");
        indexer.Index(file);
    }

    internal class NotIMetadataAware
    {

    }
}
