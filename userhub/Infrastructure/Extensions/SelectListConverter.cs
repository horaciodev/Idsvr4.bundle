using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace userhub.Infrastructure.Extensions
{
    public static class SelectListConverter{

        public static List<SelectListItem> ConvertList<T>(this IList<T> sourceList, string textField, string valueField) where T: class
        {
            Type sourceType = sourceList[0].GetType(); //is this the right way to do this? what of the list is empty
            PropertyInfo textProp = sourceType.GetProperty(textField);
            PropertyInfo valueProp = sourceType.GetProperty(valueField);

            var retList = sourceList.Select(x=> 
                      new SelectListItem(){
                          Text = textProp.GetValue(x,null).ToString(),
                          Value = valueProp.GetValue(x,null).ToString()
                      });

            return retList.ToList();

        }
    }
}