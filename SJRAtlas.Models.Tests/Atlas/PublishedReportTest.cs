using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Models.Tests.Atlas
{
    [TestFixture]
    public class PublishedReportTest : PublicationTest
    {
        protected override Publication CreatePublication()
        {
            return new PublishedReport();
        }
    }
}
