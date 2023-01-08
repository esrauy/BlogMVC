using Blog_BusinessLayer;
using Entites.ViewModels;
using Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog_WebUI.Models;
using Blog_WebUI.Filter;

namespace Blog_WebUI.Controllers
{
    [HandleException]
    public class HomeController : Controller
    {
        // GET: Home
        // Test test = new Test();

        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private BlogUserManager blogUserManager = new BlogUserManager();
        public ActionResult Index()
        {
            //int a = 0;
            //int b = 5;
            //int c = b / a;
            //[HandleException] kullandığımız zamana, normalde yukarıda bir hata vermesi gerekiyor ve bizi HandleException class'ı içinde tanımladığımız controller ve action'a yönlendirip, hata mesajını ekrana yazması gerekiyor. projeyi yukarıda IIS Express ile çalıştırdığımızda development modda olduğu için o sayfaya yönlendirmeyecek ve direk hatanın bulunduğu sayfa ve satıra gidecektir. Bu sayfayı test edebilmek için Index.cshtml sayfasında sağ tuş => view in browser ile çalıştırdığımızda bu runtime hatası HandleException yakalanacak ve HasError view'i görüntülenecek.
            return View(noteManager.ListQueryable().Where(x=> x.IsDraft == false).OrderByDescending(x=> x.ModifiedDate).ToList());
        }

        public ActionResult MostLiked()
        {
            //en beğenilenler
            
            return View("Index",noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult SelectCategory(int id)
        {           
            Category category = categoryManager.Find(x=> x.Id == id);

            return View("Index", category.Notes.OrderByDescending(x=> x.ModifiedDate).ToList());
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //giriş kontrolü
            //anasayfaya yönlendirme
            //kullanıcı bilgilerini session'a aktarma..
            if(ModelState.IsValid)
            {
                BusinessLayerResult<BlogUser> blResult = blogUserManager.LoginUser(model);
                //eğer hata varsa blResult  içinde errors list'e eklenmiş olacak. bunun kontrolünü yapıyorum.
                if(blResult.Errors.Count>0)
                {
                    //hata mesajlarını modelState'e ekliyorum. hatalar ekranda görünecek.
                    blResult.Errors.ForEach(x=> ModelState.AddModelError("", x)); // x=> blResult error içindeki her bir değer
                    return View(model);
                }
                //session'da kullanıcının bilgilerini saklıyorum.
                //Session["login"] = blResult.Result;
                CurrentSession.Set<BlogUser>("login", blResult.Result);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            //Session.Clear();
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                 BusinessLayerResult<BlogUser> blResult = blogUserManager.RegisterUser(model);
                if(blResult.Errors.Count > 0)
                {
                    //kod buraya girdiyse bu durumda email ya da kullanıcı adı kullanılıyor demektir. bu hata mesajlarını da ekrana yazdırmam gerekiyor ve kullanıcıyı uyarmam gerekiyor.
                    blResult.Errors.ForEach(x=> ModelState.AddModelError("", x)); // x= errorList'in içindeki her bir değeri temsil eder
                    //AddModelError içindeki hata mesajlarını ekranda görebiliyorum ama BussinessLayerResult içindeki Errors List'ten gelen hata mesajlarını göremiyorum. yukarıdaki kod ile error list'teki mesajları AddModelError içine eklemiş oluyorum.
                    return View(model);
                }
                //hata yok ve kayıt başarılı ise:
                return RedirectToAction("RegisterSuccess");
            }
            return View(model);
        }
        public ActionResult RegisterSuccess()
        {
            return View();
        }
        public ActionResult UserActivate(Guid id)
        {
            //maile gelen aktivasyon linkine tıklandığında çalışacak olan action burasıdır.
           BusinessLayerResult<BlogUser> blResult = blogUserManager.UserActivate(id);
            if(blResult.Errors.Count>0)
            {
                TempData["errors"] = blResult.Errors;
                return RedirectToAction("ActivateUserCancel");
            }
            return RedirectToAction("ActivateUserOk");
        }
        public ActionResult ActivateUserOk()
        {
            return View();
        }
        public ActionResult ActivateUserCancel()
        {
            List<string> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<string>;
            }
            return View(errors);
        }
        [Auth]
        public ActionResult ShowProfile()
        {
            //BlogUser currentUser = Session["login"] as BlogUser;           
            BlogUser currentUser = CurrentSession.User;
            BusinessLayerResult<BlogUser> blResult = blogUserManager.GetUserById(currentUser.Id);
            if(blResult.Errors.Count>0)
            {
                return View("ProfileLoadError", blResult.Errors);
            }
            return View(blResult.Result);
        }
        [Auth]
        [HttpGet]
        public ActionResult EditProfile()
        {
            BlogUser currentUser = CurrentSession.User;
            BusinessLayerResult<BlogUser> blResult = blogUserManager.GetUserById(currentUser.Id);
            if(blResult.Errors.Count>0)
            {
                return View("ProfileLoad", blResult.Errors);
            }
            return View(blResult.Result);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(BlogUser user, HttpPostedFileBase ProfileImage)
        {
            //HttpPostedFileBase ile gönderilen  dosyayı alabilmem için bu türde bir parametre tanımlamam ya da eklemem gerekiyor. Değişkenin ismi (ProfileImage), View tarafında input içerisinde name'e verdiğim değer ile aynı olmalı.
            //gönderilen dosyanın türünü kontrol etmem gerekiyor. uzantısının jpg, jpeg, png türünde olup olmadığınnı kontrol etmeliyim.  ve son olarak da veritabanına hangi isim ile kaydedeceksem o ismi oluşturmalıyım ve daha sonra surver tarafında images klasörünün altına bu fotoğrafı bu isimle kaydetmeliyim.
            //dosya türünün kokntrolünü ContentType ile yapıyorum
            ModelState.Remove("ModifiedUserName");
            if(ModelState.IsValid)
            {
                if(ProfileImage != null && (
                    ProfileImage.ContentType == "image/jpg" || 
                    ProfileImage.ContentType == "image/jpeg" || 
                    ProfileImage.ContentType == "image/png"))
                {
                    string fileName = $"user_{user.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    // user_10.jpeg(.jpg - .png) gibi bir isim oluşuyor.
                    //aşağıdaki kod ile birlikte fotoğrafı, server'daki images klasörünün altına oluşturduğum dosya ismi ile kopyalıyorum.
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{ fileName}"));
                    //son olarak da dosya adının veritabanında tutulması gerekiyor.
                    user.UserProfileImage = fileName;
                }
                //artık view'den gelen değişiklikleri veritabanına kaydetmek için gerekli kodları yazacağım.
                BusinessLayerResult<BlogUser> blResult = blogUserManager.UpdateProfile(user);
                if( blResult.Errors.Count>0)
                {
                    // hata oluştu demektir
                    blResult.Errors.ForEach(x=> ModelState.AddModelError("", x));
                    return View(blResult.Result);
                }
                //hata yok ise
                //Session["login"] = blResult.Result;
                CurrentSession.Set<BlogUser>("login", blResult.Result);
                return RedirectToAction("ShowProfile");
            }
            return View(user);
        }
        [Auth]
        public ActionResult DeleteProfile()
        {
            BlogUser currentUser = CurrentSession.User;
            BusinessLayerResult<BlogUser> blResult = blogUserManager.DeleteUser(currentUser.Id);
            if(blResult.Errors.Count>0)
            {
                blResult.Errors.ForEach(x=> ModelState.AddModelError("", x));
                return View("ProfileLoadError", blResult.Errors);
            }
            //Session.Clear();
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}