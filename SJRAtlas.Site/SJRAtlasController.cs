using System;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using SJRAtlas.Site.Helpers;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using SJRAtlas.Site.Models;

namespace SJRAtlas.Site
{
    [Helper(typeof(BreadCrumbHelper), "BreadCrumbs")]
    [Helper(typeof(ResourceHelper), "Resource")]
    public class SJRAtlasController : SmartDispatcherController
    {
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
    }
}
