using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NUnit.Framework;

namespace SJRAtlas.Models.Tests
{
    public class TestHelper
    {
        public static ErrorSummary TestProperties(object instance, Dictionary<string, object> propertiesAndValues)
        {
            ErrorSummary summary = new ErrorSummary();
            foreach(KeyValuePair<string, object> entry in propertiesAndValues)
            {
                ErrorSummary innerSummary = TestProperty(instance, entry.Key, entry.Value);
                foreach (string error in innerSummary)
                {
                    summary.Add(error);
                }
            }
            return summary;
        }

        public static ErrorSummary TestProperty(object instance, string propertyName, object testValue)
        {
            ErrorSummary summary = new ErrorSummary();
            try
            {
                PropertyInfo property = instance.GetType().GetProperty(propertyName);
                if (property == null)
                    throw new Exception(String.Format("The property {0} does not exist", propertyName));

                MethodInfo getter = property.GetGetMethod();
                MethodInfo setter = property.GetSetMethod();

                if (Object.Equals(getter.Invoke(instance, new object[0]), testValue))
                    summary.Add("This test of property " + propertyName + " will not work as expected since test value is already equal to actual value.");

                setter.Invoke(instance, new object[] { testValue });

                if (!Object.Equals(getter.Invoke(instance, new object[0]), testValue))
                    summary.Add(String.Format("Property value for {0} should be {1} but was {2}", propertyName, testValue.ToString(), getter.Invoke(instance, new object[0])));
            }
            catch (Exception e)
            {
                summary.Add("Error on property " + propertyName + ": " + e.Message);
            }

            return summary;
        }

        public class ErrorSummary : List<string>
        {
            public string GetSummary()
            {
                StringBuilder sb = new StringBuilder();
                foreach (string error in this)
                {
                    sb.AppendLine(error);
                }
                return sb.ToString();
            }
        }
    }
}
