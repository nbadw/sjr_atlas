using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using SJRAtlas.Models;

[TestFixture]
public class TabularPresentationFilterTest
{
    [Test]
    public void TestDrainageCodeFilter()
    {
        //string command = "SELECT * FROM tableOrView";
        //string drainageCode = "01-02-03-04-05-06";

        //TabularPresentationFilter filter = new TabularPresentationFilter();
        //filter.DrainageCode = drainageCode;

        //Assert.AreEqual("SELECT * FROM (" + command + ") WHERE DrainageCd LIKE ?", filter.FilterQuery(command));
    }

    [Test]
    public void TestInvalidDrainageCodeFilter()
    {
        Assert.Fail();
    }

    [Test]
    public void TestName()
    {
        Assert.Fail();
    }
}
