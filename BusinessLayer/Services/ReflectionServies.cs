using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    //Сервис для генерации строки запроса в url
    public static class ReflectionServies
    {
        // Принимет объект и новые объекты свойства-значение
        public static string GetQueryString<T>(T obj,params (string,string)[] keyValuePairs)
        {
            string queryString = string.Empty;
            if (obj != null)
            {
                //Перебор всех свойств
                foreach (var prop in typeof(T).GetProperties())
                {
                    //Значение текущего свойства из списка
                    var keyValue = keyValuePairs.FirstOrDefault(x => x.Item1 == prop.Name);
                    if (keyValue.Item1!= null)
                        queryString += $"{obj.GetType().Name}.{prop.Name}={keyValue.Item2}&";
                    //Если нет указаноо значения, добовляем из свойст объекта, если оно пустое не добавляем
                    else if (prop.GetValue(obj)!=null && prop.GetValue(obj).ToString() !="")
                        queryString += $"{obj.GetType().Name}.{prop.Name}={prop.GetValue(obj)}&";
                    //Записываем полное имя, {Наименование типа}{Наименование свойтва}{Значение}
                }
                if (queryString.Last() =='&') queryString=queryString.Remove(queryString.Count()-1);

            }
            return queryString;
        }
    }
}
