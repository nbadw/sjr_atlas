namespace SJRAtlas.Site
{
	using System;
	using System.Web;
    using Castle.Windsor;
    using Castle.Core.Logging;
    using Castle.Components.Common.EmailSender;
    using System.Text;

    public class GlobalApplication : HttpApplication, IContainerAccessor
	{
        private static GlobalApplication instance;
        private IWindsorContainer container;
        private ILogger logger;

		public GlobalApplication() 
        {
            instance = this;
            container = new WebApplicationContainer();
            logger = CreateLogger(GetType());
        }

		void Application_Start(object sender, EventArgs evt)
		{
            logger.Info("##### SJRATLAS APPLICATION STARTED #####");
		}

        void Application_End(object sender, EventArgs evt)
        {
            logger.Info("###### SJRATLAS APPLICATION ENDED ######");
            container.Dispose();
        }

        void Application_Error(object sender, EventArgs evt)
        {
            Exception e = HttpContext.Current.Server.GetLastError();
            logger.Error("Uncaught exception", e);

            try
            {
                IEmailSender emailer = (IEmailSender)container.GetService(typeof(IEmailSender));
                emailer.Send(CreateErrorEmail(e));
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("Error sending e-mail", ex);
                }
            }            
            
            //context.Server.ClearError();
            // TODO: redirect to a 404
        }

        #region IContainerAccessor Members

        public IWindsorContainer Container
        {
            get { return container; }
        }

        #endregion

        public static Message CreateErrorEmail(Exception e)
        {
            HttpContext context = HttpContext.Current;
            StringBuilder body = new StringBuilder();

            body.AppendFormat("The following error occured at {0}\n", DateTime.Now.ToString("MMMM dd, yyyy - hh:mm:ss tt"))
                .AppendFormat("URL:        {0}\n", context.Request.RawUrl)
                .AppendFormat("Referer:    {0}\n", context.Request.Headers["Referer"])
                .AppendFormat("User-Agent: {0}\n\n", context.Request.Headers["User-Agent"]);

            while (e != null)
            {
                body.Append(e.Message)
                    .Append("\n")
                    .Append(e.StackTrace)
                    .Append("\n\n");

                e = e.InnerException;
            }

            return new Message("ccasey@unb.ca", "ccasey@unb.ca", "Application Error", body.ToString());

            try
            {
                IEmailSender sender = (IEmailSender)ServiceProvider.GetService(typeof(IEmailSender));

                sender.Send(message);
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("Error sending e-mail", ex);
                }

                throw new RailsException("Error sending e-mail", ex);
            }
        }

        public static ILogger CreateLogger(string name)
        {
            return LoggerFactory.Create(name);
        }

        public static ILogger CreateLogger(Type type)
        {
            return LoggerFactory.Create(type);
        }

        private static ILoggerFactory LoggerFactory
        {
            get 
            { 
                ILoggerFactory nullLoggerFactory = new NullLogFactory();
                if (ContainerAccessor == null || ContainerAccessor.Container == null)
                    return nullLoggerFactory;

                ILoggerFactory loggerFactory = ContainerAccessor.Container[typeof(ILoggerFactory)] as ILoggerFactory;
                return loggerFactory != null ? loggerFactory : nullLoggerFactory;
            }
        }
        
        public static IContainerAccessor ContainerAccessor
        {
            get { return instance; }
        }
    }
}
