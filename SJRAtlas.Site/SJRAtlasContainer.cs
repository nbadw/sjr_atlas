using System;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using SJRAtlas.Site.Controllers;
using Castle.Core.Logging;
using Castle.MonoRail.WindsorExtension;
using Castle.MonoRail.Framework.Routing;

namespace SJRAtlas.Site
{
    public class SJRAtlasContainer : WindsorContainer
    {
        private ILogger logger;

        public SJRAtlasContainer()
            : base(new XmlInterpreter(new ConfigResource()))
        {
            logger = (this[typeof(ILoggerFactory)] as ILoggerFactory).Create(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            RegisterFacilities();
            RegisterComponents();
            //RegisterRoutes();
        }

        protected virtual void RegisterFacilities()
        {

        }

        protected virtual void RegisterComponents()
        {     
            // Load controllers automatically
            Type[] controllerTypes = System.Reflection.Assembly.GetCallingAssembly().GetTypes();
            foreach (Type type in controllerTypes)
            {
                if (!type.Equals(typeof(SJRAtlasController)) && type.IsSubclassOf(typeof(Castle.MonoRail.Framework.Controller)))
                {
                    string id = type.Name.ToLower().Replace("controller", "");
                    logger.Debug("Registering Controller " + type.ToString() + " as " + id + ".controller");
                    if(!Kernel.HasComponent(type))
                        AddComponent(id + ".controller", type);
                }
            }
        }

        protected virtual void RegisterRoutes()
        {
            RoutingModuleEx.Engine.Add(
                new PatternRoute("/search/<action>/[params]").
                DefaultForController().Is("search").
                DefaultForArea().IsEmpty.
                DefaultForAction().Is("advanced")
            );
            RoutingModuleEx.Engine.Add(
                new PatternRoute("/<action>").
                DefaultForController().Is("site").
                DefaultForArea().IsEmpty.
                DefaultForAction().Is("index")
            );
        }
    }
}
