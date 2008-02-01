using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("feedback")]
    public class Feedback : ActiveRecordValidationBase<Feedback>
    {
        private int id;
        private string subject;
        private string senderName;
        private string message;
        private string refererUrl;
        private string senderEmail;
        private string userAgent;
        private DateTime createdOn;

        public Feedback()
        {
            createdOn = DateTime.Now;
        }

        [PrimaryKey(PrimaryKeyType.Increment, "id")]
        public virtual int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        [Property("subject")]        
        public virtual string Subject
        {
            get { return this.subject; }
            set { this.subject = value; }
        }

        [Property("sender_name")]
        public virtual string SenderName
        {
            get { return this.senderName; }
            set { this.senderName = value; }
        }

        [Property("sender_email")]
        [ValidateEmail("The email address appears to be invalid")]
        [ValidateNonEmpty("Please fill in your email")]
        public string SenderEmail
        {
            get { return senderEmail; }
            set { senderEmail = value; }
        }    

        [Property("message")]
        [ValidateNonEmpty("You message cannot be blank")]
        public virtual string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        [Property("referer_url")]
        public virtual string RefererUrl
        {
            get { return this.refererUrl; }
            set { this.refererUrl = value; }
        }

        [Property("user_agent")]
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }

        [Property("created_on")]
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
    }
}
