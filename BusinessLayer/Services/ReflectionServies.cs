using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public static class ReflectionServies
    {
        public static string GetQueryString<T>(T obj,params (string,string)[] keyValuePairs)
        {
            string queryString = string.Empty;
            if (obj != null)
            {
                foreach (var prop in typeof(T).GetProperties())
                {
                    var keyValue = keyValuePairs.FirstOrDefault(x => x.Item1 == prop.Name);
                    if (keyValue.Item1!= null)
                        queryString += $"{prop.Name}={keyValue.Item2}&";
                    else if(prop.GetValue(obj)!=null && prop.GetValue(obj).ToString() !="")
                        queryString += $"{prop.Name}={prop.GetValue(obj)}&";
                    var x = prop.GetValue(obj);
                }
                if (queryString.Last() =='&') queryString=queryString.Remove(queryString.Count()-1);

            }
            Console.WriteLine("query =  "+ queryString);
            return queryString;
        }
    }
}
