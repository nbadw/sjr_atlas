using System;
using Castle.MonoRail.Framework;

namespace SJRAtlas.Site.Filters
{
    public class LoadingFilter : IFilter
    {
        public bool Perform(ExecuteEnum exec, IRailsEngineContext context, Controller controller)
        {
            return true;
        }
    }
}
