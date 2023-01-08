using Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog_WebUI.Models
{
    public class CurrentSession
    {
        public static BlogUser User
        {
            //bu metodu static yapıyorum. new'lemeden ulaşabilmek için
            //session'da bulunan kullanıcının alınması için kullanılacak.
            get
            {
              //if  (HttpContext.Current.Session["login"] != null)
              //  {
              //      return HttpContext.Current.Session["login"] as BlogUser;
              //  }
              return Get<BlogUser>("login");
            }
        }

        //aşağıdaki meetotta, generic bir yapı kullandım. Sadece blogUser türünde değil, farklı türden verileri de session'a koyabilmek için kullanacağım metottur.
        //currentSession.Set<string>("name", "esra")
        //currentSession.Set<BlogUser>("login", [BlogUser türündeki nesneyi vereceğiz])

        public static void Set<T>(string key, T obj)
        {
             HttpContext.Current.Session[key] = obj;
        }

        // session'daki veriyi almak için kullanacağım generic Get metodu.
        public static T Get<T>(string key)
        {
            if(HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }
            return default(T);
        }

        //session'da bulunan bir veriyi parametresini vererek session'dan kaldırmak ya da silmek için kullanacağım metot.
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        //session'daki bütün veriyi temizliyor.

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
// session'da verinin saklanma süresi 20 dakikadır. eğer sayfada herhangi bir işlem olmazsa ve 20 dakika geçerse bu bilgi/veri session'dan silinir.
//session'da tutulacak verinin tutulma süresini değiştirebiliriz. => web.config dosyası içinde
// timeout'a verdiğimiz sayısal değer kadar dakika sessionda tutulacak.
// <system.web>
// <sessionState mode = "InProc" timeout = "60">
// <system.web>