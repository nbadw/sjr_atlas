using System;
using NUnit.Framework;
using Castle.MonoRail.TestSupport;
using SJRAtlas.Site.Controllers;
using SJRAtlas.Site.Models;
using Castle.MonoRail.Framework;
using Castle.Components.Validator;
using Castle.Components.Common.EmailSender;
using Rhino.Mocks;

namespace SJRAtlas.Site.Tests.Controllers
{
    [TestFixture]
    public class FeedbackControllerTest : BaseControllerTest
    {

        [Test]
        public void TestValidationFailsAndFormIsDisplayedWithErrorMessages()
        {
            //Feedback feedback = new Feedback();
            //SimulateOneValidationErrorFor(controller, feedback);
            //controller.SubmitFeedback(feedback);

            //Assert.AreEqual("/feedback/feedbackform.rails", Response.RedirectedTo);

            //Assert.IsNotNull(controller.Flash["feedback"]);
            //Assert.IsNotNull(controller.Flash["summary"]);
            //Assert.AreEqual(1, ((ErrorSummary)controller.Flash["summary"]).ErrorsCount);
            Assert.Ignore();
        }

        [Test]
        public void TestFeedbackIsSavedToDatabaseAndThenEmailedSucceeds()
        {
            //Feedback feedback = mocks.CreateMock<Feedback>();
            //controller.FeedbackRecipients = new string[] { "recipient-01@test.com", "recipient-02@test.com", "recipient-03@test.com" };

            //feedback.CreateAndFlush();
            //LastCall.On(feedback).Repeat.Once();
            //Expect.Call(feedback.Subject).Repeat.Any().Return("Email Test");
            //Expect.Call(feedback.SenderName).Repeat.Any().Return("Test Sender");

            //mocks.ReplayAll();
            
            //controller.SubmitFeedback(feedback);

            //Console.WriteLine(((Castle.MonoRail.Framework.Test.MockRailsEngineContext)Context).RenderedEmailTemplates[0].Name);

            //Assert.IsTrue(HasRenderedEmailTemplateNamed("feedback"));
            //Assert.AreEqual(@"feedback\EmailSent", controller.SelectedViewName);
            //Assert.AreEqual(controller.FeedbackRecipients.Length, MessagesSent.Length);
            //for (int i = 0; i < controller.FeedbackRecipients.Length; i++)
            //{
            //    Assert.AreEqual(controller.FeedbackRecipients[i], MessagesSent[i].To);
            //}

            //mocks.VerifyAll();
            Assert.Ignore();
        }

        [Test]
        public void TestFeedbackIsSavedToDatabaseAndThenEmailedFails()
        {
            Assert.Ignore();
        }

        private void SimulateOneValidationErrorFor(SmartDispatcherController controller, object instance)
        {
            controller.ValidationSummaryPerInstance.Add(instance, CreateDummyErrorSummaryWithOneError());
        }

        private ErrorSummary CreateDummyErrorSummaryWithOneError()
        {
            ErrorSummary errors = new ErrorSummary();
            errors.RegisterErrorMessage("blah", "blah");

            return errors;
        }
    }
}
