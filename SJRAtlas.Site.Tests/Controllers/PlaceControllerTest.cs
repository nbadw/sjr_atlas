using System;
using Castle.MonoRail.TestSupport;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;
using SJRAtlas.Site.Controllers;
using SJRAtlas.Models;
using Castle.ActiveRecord;
using System.Collections;

namespace SJRAtlas.Site.Tests.Controllers
{
    [TestFixture]
    public class PlaceControllerTest : BaseControllerTest
    {
        private MockRepository mocks = new MockRepository();

        [Test]
        public void TestViewPlace()
        {
            string id = "ABCDE";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            Place place = mocks.CreateMock<Place>();
            place.CgndbKey = id;

            Expect.Call(mediator.Find<Place>(id)).Return(place);
            Expect.Call(place.IsWithinBasin()).Repeat.Any().Return(false);
            Expect.Call(place.RelatedInteractiveMaps).Return(new List<InteractiveMap>());
            Expect.Call(place.RelatedPublications).Return(new List<Publication>());
            mocks.ReplayAll();

            PlaceController controller = new PlaceController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "place", "view");
            controller.View(id);

            Assert.AreEqual(place, controller.PropertyBag["place"]);
            Assert.IsNotNull(controller.PropertyBag["interactive_maps"]);
            Assert.IsNotNull(controller.PropertyBag["published_maps"]);
            Assert.IsNotNull(controller.PropertyBag["published_reports"]);
            Assert.AreEqual(@"place\view", controller.SelectedViewName);
            mocks.VerifyAll();

        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewPlaceThrowsExceptionWhenCgndbKeyParameterIsNull()
        {
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            PlaceController controller = new PlaceController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "place", "view");
            controller.View(null);
        }

        [Test]
        [ExpectedException(typeof(NotFoundException))]
        public void TestViewPlaceThrowsExceptionWhenPlaceDoesNotExist()
        {
            string id = "ABCDE";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();

            Expect.Call(mediator.Find<Place>(id)).Throw(new NotFoundException("Record Not Found"));
            mocks.ReplayAll();

            PlaceController controller = new PlaceController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "place", "view");
            controller.View(id);

            mocks.VerifyAll();
        }

        [Test]
        public void TestRedirectsToWaterBodyViewWhenPlaceNameIsWaterBody()
        {
            string id = "ABCDE";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            Place place = mocks.CreateMock<Place>();
            place.CgndbKey = id;
            IList<InteractiveMap> basinMaps = new List<InteractiveMap>();

            Expect.Call(mediator.Find<Place>(id)).Return(place);
            Expect.Call(place.IsWithinBasin()).Return(true);
            Expect.Call(mediator.WaterBodyExistsForCgndbKeyOrAltCgndbKey(id)).Return(true);            
            mocks.ReplayAll();

            PlaceController controller = new PlaceController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "view", "place");
            controller.View(id);
            Assert.IsTrue(Response.WasRedirected);
            Assert.AreEqual("/waterbody/view.rails?cgndbKey=" + id + "&", Response.RedirectedTo);
            mocks.VerifyAll();
        }

        [Test]
        public void TestRedirectsToWatershedViewWhenPlaceNameIsWatershed()
        {
            string id = "ABCDE";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            Place place = mocks.CreateMock<Place>();
            place.CgndbKey = id;
            IList<InteractiveMap> basinMaps = new List<InteractiveMap>();

            Expect.Call(mediator.Find<Place>(id)).Return(place);
            Expect.Call(place.IsWithinBasin()).Return(true).Repeat.Twice();
            Expect.Call(mediator.WaterBodyExistsForCgndbKeyOrAltCgndbKey(id)).Return(false);
            Expect.Call(mediator.WatershedExistsForCgndbKey(id)).Return(true);
            mocks.ReplayAll();

            PlaceController controller = new PlaceController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "view", "place");
            controller.View(id);
            Assert.IsTrue(Response.WasRedirected);
            Assert.AreEqual("/watershed/view.rails?cgndbKey=" + id + "&", Response.RedirectedTo);
            mocks.VerifyAll();
        }
    }
}
