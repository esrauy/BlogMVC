using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_WebUI.Filter
{
    public class HandleException : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            //eğer uygulamanın herhangi bir sayfasında bir hata oluşur da exception fırlatırsa, sistemin göstereceği hata mesajı yerine benim tasarladığım sayfa ve hata mesajı kullanıcıya gösterilecek.
            filterContext.Controller.TempData["LastError"] = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult("/Home/HasError");
        }
    }
}