using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Castle.MonoRail.Framework;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

namespace SJRAtlas.Site.Filters
{
    public class EscapeToUnicodeFilter : IFilter
    {
        #region IFilter Members

        public bool Perform(ExecuteEnum exec, IRailsEngineContext context, Controller controller)
        {
            Console.WriteLine("running filter");
            IDictionary propertyBag = controller.PropertyBag;
            Hashtable newValues = new Hashtable();
            foreach (object key in propertyBag.Keys)
            {
                object value = propertyBag[key];
                if (value is string)
                    newValues[key] = EscapeToUnicode((string)value);
            }

            foreach (object key in newValues.Keys)
            {
                propertyBag[key] = newValues[key];
            }

            return true;
        }

        private string EscapeToUnicode(string value)
        {
            Console.WriteLine(value);
            StringBuilder newValue = new StringBuilder();
            foreach(char c in value.ToCharArray())
            {
                Console.WriteLine((int)c);
                if((int)c > 127)
                    newValue.AppendFormat("&#{0};", (int)c);
                else
                    newValue.Append(c);
            }
            Console.WriteLine(newValue.ToString());
            return newValue.ToString();
        }

        #endregion
    }
}
