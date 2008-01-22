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
            container = new SJRAtlasContainer();
            //IWindsorContainer container = new RhinoContainer("Windsor.boo");
            logger = (container[typeof(ILoggerFactory)] as ILoggerFactory).Create(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        public static ILoggerFactory LogFactory
        {
            get { return ContainerAccessor.Container[typeof(ILoggerFactory)] as ILoggerFactory; }
        }

        public static ILogger CreateLogger(string name)
        {
            return LogFactory.Create(name);
        }

        public static IContainerAccessor ContainerAccessor
        {
            get { return instance; }
        }
    }
}
