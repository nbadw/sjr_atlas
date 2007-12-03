using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Functional.Tests
{
    [TestFixture]
    public class CachedGnssTest : BaseSeleniumTest
    {
        //[Test]
        public void TestQuickSearchForArnoldBrookAndUserSelectsTheResultNotInTheBasin()
        {
            string[] cgndbKeys = 
            {
                "DAAKJ", "DAANA", "DAANV", "DABEX", "DABJB", "DABJO", "DABJT", "DABLG", "DABLI", "DABNQ",
                "DABNU", "DABUT", "DABUW", "DABWA", "DABWE", "DABXB", "DABXH", "DABXR", "DABXZ", "DABYV",
                "DACRU", "DACYH", "DADAB", "DADFU", "DADLD", "DADLO", "DADTL", "DADTP", "DADWD", "DAEAH"
            };

            Random generator = new Random();
            int iterations = cgndbKeys.Length * 10;
            for (int i = 0; i < iterations; i++)
            {
                int random = generator.Next(cgndbKeys.Length - 1);
                Open(String.Format("/placename/view.rails?id={0}", cgndbKeys[random]));
            }
        }	
    }
}
