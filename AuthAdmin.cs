using Blog_WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_WebUI.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        //admin olmayan kullanıcılar için,
        //category hepsi AccessDenied sayfasına yönlenmeli
        //BlogUser hepsi AccessDenied sayfasına yönlenmeli
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(CurrentSession.User != null && CurrentSession.User.IsAdmin == false)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}