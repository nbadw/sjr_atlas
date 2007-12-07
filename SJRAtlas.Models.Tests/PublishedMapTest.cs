using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class PublishedMapTest : PublicationTest
    {
        protected override Publication CreatePublication()
        {
            return new PublishedMap();
        }
    }
}
