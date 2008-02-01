using System;
using System.Collections.Generic;
using System.Text;
using Castle.MonoRail.Framework;
using Castle.Components.Common.EmailSender;
using Castle.Components.Validator;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Controllers
{
    [Layout("sjratlas"), Rescue("generalerror")]
    public class FeedbackController : BaseController
    {
        [AjaxAction]
        public void FeedbackForm()
        {
            CancelLayout();
        }

        [AjaxAction]
        [Rescue("problemsendingemail")]
        public void SubmitFeedback([DataBind("feedback", Validate = true)] Feedback feedback)
        {
            Logger.Debug("Feedback form has been submitted");
            if (HasValidationError(feedback))
            {
                Flash["feedback"] = feedback;
                Flash["summary"] = GetErrorSummary(feedback);
                RedirectToAction("feedbackform");        
                
                if(Logger.IsDebugEnabled)
                {
                    StringBuilder sb = new StringBuilder("Validation errors found on the form:\n");
                    foreach(string error in GetErrorSummary(feedback).ErrorMessages)
                    {
                        sb.AppendLine(error);
                    }
                    Logger.Debug(sb.ToString());
                }
                return;
            }

            feedback.RefererUrl = Context.Request.Headers["Referer"];
            feedback.UserAgent = Context.Request.Headers["User-Agent"];

            Logger.Debug("Saving Feedback object to database");
            feedback.CreateAndFlush();
            Logger.Debug("Feedback saved");
            
            PropertyBag["to"] = "Saint John Atlas Dev Team";
            PropertyBag["sender_name"] = feedback.SenderName;
            PropertyBag["sender_email"] = feedback.SenderEmail;
            PropertyBag["subject"] = feedback.Subject;
            PropertyBag["message"] = feedback.Message;
            PropertyBag["referer_url"] = feedback.RefererUrl;
            PropertyBag["user_agent"] = feedback.UserAgent;

            foreach (string recipient in FeedbackRecipients)
            {
                Message message = RenderMailMessage("feedback");
                message.From = feedback.SenderEmail;
                message.Subject = feedback.Subject;
                message.To = recipient;
                Logger.Debug("Attempting to send feedback message to " + recipient);
                DeliverEmail(message);
            }

            CancelLayout();
            RenderView("EmailSent");
            Logger.Info("Feedback has been saved and emailed successfully.");
        }

        private string[] feedbackRecipients = new string[] { "ccasey@unb.ca", "ardw@nbnet.nb.ca", "a289o@unb.ca" };

        public string[] FeedbackRecipients
        {
            get { return feedbackRecipients; }
            set { feedbackRecipients = value; }
        }
    }
}
