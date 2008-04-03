using System;
using System.Web;
using Castle.Windsor;
using Castle.Core.Logging;
using System.Collections.Generic;
using ESRI.ArcGIS.Server;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;

/// <summary>
/// Summary description for GlobalApplication
/// </summary>
public class GlobalApplication : HttpApplication, IContainerAccessor
{
    private static GlobalApplication instance;
    private IWindsorContainer container;
    private ILogger logger;

    public GlobalApplication()
    {
        instance = this;
        container = new WindsorContainer(
            new XmlInterpreter(new ConfigResource())
        );
        logger = CreateLogger(GetType());
    }

    #region Application & Session Events

    void Application_Start(Object sender, EventArgs e)
    {
        logger.Info("##### MAPPING APPLICATION STARTED #####");
    }

    void Application_End(Object sender, EventArgs e)
    {
        logger.Info("###### MAPPING APPLICATION ENDED ######");
        container.Dispose();
    }

    void Application_Error(Object sender, EventArgs e)
    {
        logger.Error("!!! UNHANDLED EXCEPTION", Server.GetLastError());
    }

    void Session_End(Object sender, EventArgs e)
    {
        ReleaseServerContexts();
    }

    void Session_Abandon(Object sender, EventArgs e)
    {
        ReleaseServerContexts();
    }

    private void ReleaseServerContexts()
    {
        logger.Debug("###### RELEASING SERVER CONTEXTS ######");
        List<IServerContext> contexts = new List<IServerContext>();
        for (int i = 0; i < Session.Count; i++)
        {
            if (Session[i] is IServerContext)
                contexts.Add((IServerContext)Session[i]);
            else if (Session[i] is IDisposable)
                ((IDisposable)Session[i]).Dispose();
        }

        foreach (IServerContext context in contexts)
        {
            context.RemoveAll();
            context.ReleaseContext();
        }
    }

    #endregion

    #region IContainerAccessor Members

    public IWindsorContainer Container
    {
        get { return container; }
    }

    #endregion

    #region Container Utility Methods

    private ServerContext serverContext;

    public ServerContext ServerContext
    {
        get { return serverContext; }
        set { serverContext = value; }
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

    #endregion
}
