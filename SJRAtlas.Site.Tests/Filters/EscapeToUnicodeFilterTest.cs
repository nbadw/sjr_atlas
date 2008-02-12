using System;
using NUnit.Framework;
using SJRAtlas.Site.Filters;
using Castle.MonoRail.Framework;
using Castle.MonoRail.TestSupport;
using System.Text;

namespace SJRAtlas.Site.Tests.Filters
{
    [TestFixture]
    public class EscapeToUnicodeFilterTest : BaseControllerTest
    {
        [Test]
        public void TestWhenCharactersNeedToBeEscaped()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append((char)128);
            builder.Append((char)233);
            builder.Append("é");
            string testValue = builder.ToString();

            TestFilterController controller = new TestFilterController();
            PrepareController(controller, "", "testfilter", "dotest");
            controller.DoTest(testValue);

            EscapeToUnicodeFilter filter = new EscapeToUnicodeFilter();
            filter.Perform(ExecuteEnum.AfterAction, Context, Context.CurrentController);

            Assert.AreEqual("&#128;&#233;&#233;", controller.PropertyBag[testValue]);
        }

        [Test]
        public void TestWhenCharactersDoNotNeedToBeEscaped()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append((char)0);
            builder.Append((char)127);
            string testValue = builder.ToString();

            TestFilterController controller = new TestFilterController();
            PrepareController(controller, "", "testfilter", "dotest");
            controller.DoTest(testValue);

            EscapeToUnicodeFilter filter = new EscapeToUnicodeFilter();
            filter.Perform(ExecuteEnum.AfterAction, Context, Context.CurrentController);

            Assert.AreEqual(testValue, controller.PropertyBag[testValue]);
        }

        [FilterAttribute(ExecuteEnum.AfterAction, typeof(EscapeToUnicodeFilter))]
        public class TestFilterController : Controller
        {
            public void DoTest(string value)
            {
                PropertyBag[value] = value;
            }
        }
    }
}
