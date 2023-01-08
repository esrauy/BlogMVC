using Blog_WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_WebUI.Filter
{
    public class Auth : FilterAttribute, IAuthorizationFilter
    {
        // Authorization  
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(CurrentSession.User == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}