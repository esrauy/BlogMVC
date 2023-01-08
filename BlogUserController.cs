using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog_BusinessLayer;
using Blog_WebUI.Filter;
using Entites;

namespace Blog_WebUI.Controllers
{
    [Auth]
    [AuthAdmin]
    [HandleException]
    public class BlogUserController : Controller
    {
        private BlogUserManager userManager = new BlogUserManager();

        // GET: BlogUser
        public ActionResult Index()
        {
            return View(userManager.List());
        }

        // GET: BlogUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogUser blogUser = userManager.Find(x=> x.Id == id);
            if (blogUser == null)
            {
                return HttpNotFound();
            }
            return View(blogUser);
        }

        // GET: BlogUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogUser/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogUser blogUser)
        {
            if (ModelState.IsValid)
            {
                // Email kontrolü, userName kontrolü vb.
                BusinessLayerResult<BlogUser> blResult = userManager.Insert(blogUser);
                if(blResult.Errors.Count > 0)
                {
                    blResult.Errors.ForEach(x=> ModelState.AddModelError("", x));
                    return View(blogUser);
                }

                return RedirectToAction("Index");
            }

            return View(blogUser);
        }

        // GET: BlogUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogUser blogUser = userManager.Find(x=> x.Id == id);
            if (blogUser == null)
            {
                return HttpNotFound();
            }
            return View(blogUser);
        }

        // POST: BlogUser/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogUser blogUser)
        {
            if (ModelState.IsValid)
            {
                //kontroller yapılacak, sonra da update işlemi yapılacak.
                BusinessLayerResult<BlogUser> blResult = userManager.Update(blogUser);
                if(blResult.Errors.Count>0)
                {
                    blResult.Errors.ForEach(x=> ModelState.AddModelError("", x));
                    return View(blogUser);
                }
                return RedirectToAction("Index");
            }
            return View(blogUser);
        }

        // GET: BlogUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogUser blogUser = userManager.Find(x => x.Id == id);
            if (blogUser == null)
            {
                return HttpNotFound();
            }
            return View(blogUser);
        }

        // POST: BlogUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogUser blogUser = userManager.Find(x => x.Id == id);
           userManager.Delete(blogUser);
            return RedirectToAction("Index");
        }

    }
}
