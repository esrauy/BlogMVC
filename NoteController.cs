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
using Blog_WebUI.Models;
using Entites;

namespace Blog_WebUI.Controllers
{
    [HandleException]
    public class NoteController : Controller
    {
        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        LikedManager likedManager = new LikedManager();

        // GET: Note
        public ActionResult Index()
        {
            BlogUser user = CurrentSession.User;
            var notes = noteManager.ListQueryable().Include("Category").Include("Owner").Where(x => x.Owner.Id == user.Id).OrderByDescending(x => x.ModifiedDate).ToList();
            return View(notes.ToList());
        }
        [Auth]
        public ActionResult MyLiked()
        {
            var notes = likedManager.ListQueryable().Include("LikedUser").Include("Note").Where(x=> x.LikedUser.Id == CurrentSession.User.Id).Select(x=> x.Note).Include("Category").Include("Owner").OrderByDescending(x=> x.ModifiedDate);
            return View("Index", notes.ToList());
        }

        // GET: Note/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x=> x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Note/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        // POST: Note/Create
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                noteManager.Insert(note);
                return RedirectToAction("Index");
            }

            //ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId); burada veritabanına sorgu atarak çalışıyoruz. bu büyük projelerde ciddi br sorun olabilir. categoryManager.list() yerine cacheHelper.GetCategoriesFromCache() ile çalışabiliriz.
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);

            return View(note);
        }

        // GET: Note/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x=> x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // POST: Note/Edit/5
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                //TODO : BusinessLayerResult<note> kullanarak tekrardan düzenleyebiliriz. kontrol olarak da aynı kategoride aynı title olmamalı.
               Note dbNote = noteManager.Find(x=>x.Id == note.Id);
                dbNote.Title = note.Title;
                dbNote.Text = note.Text;
                dbNote.IsDraft = note.IsDraft;
                dbNote.CategoryId = note.CategoryId;
                noteManager.Update(dbNote);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.Find(x => x.Id == id);
            noteManager.Delete(note);
            return RedirectToAction("Index");
        }
        [Auth]
        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            List<int> likedNoteids = likedManager.List(x => x.LikedUser.Id == CurrentSession.User.Id && ids.Contains(x.Note.Id)).Select(x => x.Note.Id).ToList();

            return Json(new { result = likedNoteids });
        }
        [Auth]
        [HttpPost]
        public ActionResult SetNoteLike(int noteid, bool liked)
        {
            int result = 0;
            //önce kullanıcının beğendiği kayıt/note veritabanında var mı yok mu kontrolü yapılır.
            Liked like = likedManager.Find(x=> x.Note.Id == noteid && x.LikedUser.Id == CurrentSession.User.Id);
            Note note = noteManager.Find(x=> x.Id == noteid);
            if(like != null && liked == false)
            {
                result = likedManager.Delete(like);
            }
            else if(like == null && liked == true)
            {
                result = likedManager.Insert(new Liked() { LikedUser = CurrentSession.User, Note = note});
            }
            // yapılan işleme göre beğenildi ya da beğenilmedi.. bu duruma göre likeCount değiştirmemiz gerekecek.
            if(result>0)
            {
                if(liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }
                noteManager.Update(note);
                return Json(new {hasError = false, errorMessage=string.Empty, result = note.LikeCount});
            }
            return Json(new { hasError = true, errorMessage = "Beğenilme işlemi gerçekleştiirilemedi.", result = note.LikeCount });
        }

        public ActionResult ShowNoteDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialNoteDetail", note);
        }
    }
}
