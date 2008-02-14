namespace SJRAtlas.Site
{
	using System;
	using System.Web;
    using Castle.Windsor;
    using Castle.Core.Logging;

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

		public void Application_OnStart()
		{
            logger.Info("##### SJRATLAS APPLICATION STARTED #####");
		}

        public void Application_OnEnd()
        {
            logger.Info("###### SJRATLAS APPLICATION ENDED ######");
            container.Dispose();
        }

        #region IContainerAccessor Members

        public IWindsorContainer Container
        {
            get { return container; }
        }

        #endregion

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
