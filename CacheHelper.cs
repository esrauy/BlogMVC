using Blog_BusinessLayer;
using Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Blog_WebUI.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            //webHelper içinde bulunan webCache isimli class'tan faydalanacağız. bunun için de NuGet içinden Microsoft.AspNet.WebHelpers paketi projemize dahil etmemiz gerekir.

            var result = WebCache.Get("category");
            if(result == null)
            {
                CategoryManager categoryManager = new CategoryManager();
                result=categoryManager.List();
                // WebCache.Set("key", value, ne kadar cache'te kalacağına dair dakika cinsinden değer, [her kullanımda cache'te kalma süresi verilen değer kadar ötelenecek.]);
                WebCache.Set("category", result, 20, true);
            }
            return result;
        }

        public static void RemoveCategoriesFromCache()
        {
            WebCache.Remove("category");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}