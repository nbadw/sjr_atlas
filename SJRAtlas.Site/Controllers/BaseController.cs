using System;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using SJRAtlas.Site.Models;
using SJRAtlas.Models;
using SJRAtlas.Site.Helpers;

namespace SJRAtlas.Site.Controllers
{
    [Layout("sjratlas")]
    [Rescue("generalerror")]
    [Helper(typeof(AtlasHelper), "Atlas")]
    public class BaseController : SmartDispatcherController
    {
        public BaseController() : this(new AtlasMediator())
        {

        }

        public BaseController(AtlasMediator atlasMediator)
        {
            if (atlasMediator == null)
                throw new ArgumentNullException("atlasMediator");

            this.atlasMediator = atlasMediator;
        }

        private AtlasMediator atlasMediator;

        public AtlasMediator AtlasMediator
        {
            get { return atlasMediator; }
            set { atlasMediator = value; }
        }

        protected IList<T> GetPublicationsByType<T>(IList<Publication> publications) where T : Publication
        {
            IList<T> publicationsByType = new List<T>();
            foreach (Publication publication in publications)
            {
                if (publication is T)
                    publicationsByType.Add((T)publication);
            }
            return publicationsByType;
        }

        /*
        protected override void InvokeMethod(MethodInfo method, IRequest request, IDictionary actionArgs)
        {
            bool isAjaxAction = method.GetCustomAttributes(typeof(AjaxActionAttribute), false).Length > 0;
            Logger.Info("Ajax Action Requested");

            try
            {
                ParameterInfo[] parameters = method.GetParameters();                

                object[] methodArgs = BuildMethodArguments(parameters, request, actionArgs);  

                object result = method.Invoke(this, methodArgs);

                if (result != null)
                {
                    if (isAjaxAction)
                    {
                        Response.ContentType = "text/plain";
                        if (typeof(bool).IsAssignableFrom(result.GetType()))
                        {
                            RenderText(
                                (bool)result
                                    ? Newtonsoft.Json.JavaScriptConvert.True
                                    : Newtonsoft.Json.JavaScriptConvert.False
                            );
                        }
                        else
                        {
                            string json = Newtonsoft.Json.JavaScriptConvert.SerializeObject(result);
                            if (Logger.IsDebugEnabled)
                                Logger.Debug(json);
                            RenderText(json);
                        }
                    }
                    else
                    {
                        RenderText(result.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                //Error error = CreateError(e, null);
                //Exception inner = e.InnerException;

                //while (inner != null)
                //{
                //    error = CreateError(inner, error);
                //    inner = inner.InnerException;
                //}

                if (Logger.IsErrorEnabled)
                    Logger.Error("Controller Error", e);

                throw e;
            }
        }
         **/
    }
}
