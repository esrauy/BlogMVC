using Blog_Common.Helpers;
using Blog_DataAccessLayer.EntityFrameworkSQL;
using Entites;
using Entites.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BusinessLayer
{
    public class BlogUserManager : BaseManager<BlogUser>
    {
        //Repository<BlogUser> repository = new Repository<BlogUser>();  //repository'e bağlandık: gerek kalmadı

        public BusinessLayerResult<BlogUser> DeleteUser(int id)
        {

            BusinessLayerResult<BlogUser> blResult = new BusinessLayerResult<BlogUser>();
            BlogUser user = Find(x => x.Id == id);
            if(user != null)
            {
                //user'a ait note'lar silinmeli
                //user'a ait like'lar silinmeli
                //user'a ait comment'ler silinmeli

                if (Delete(user) == 0)
                {
                    blResult.Errors.Add("Kullanıcı Silinemedi.");
                    return blResult;
                }
            }
            else
            {
                blResult.Errors.Add("Kullanıcı Bulunamadı.");
            }
            return blResult;
        }

        public BusinessLayerResult<BlogUser> GetUserById(int id)
        {
            BusinessLayerResult<BlogUser> blResult = new BusinessLayerResult<BlogUser>();
            BlogUser user = Find(x=> x.Id == id);
            if(user == null)
            {
                blResult.Errors.Add("Kullanıcı Bulunamadı.");
            }
            else
            {
                blResult.Result = user;
            }
            return blResult;
        }

        public BusinessLayerResult<BlogUser> LoginUser(LoginViewModel model)
        {
           BusinessLayerResult<BlogUser> blResult = new BusinessLayerResult<BlogUser>();
            blResult.Result = Find(x=> x.UserName == model.UserName && x.Password == model.Password);
            if(blResult.Result != null)
            {
                //kullanıcı sistemde kayıtlı ise geriye bu kullanıcı bilgileri gönderilecek.
                if(!blResult.Result.IsActive)
                {
                    //kullanıcı kayıtlı ise aktif olup olmadığı kontrol edilir. aktif değilse error listesine aşağıdaki mesaj eklenir.
                    blResult.Errors.Add("Hesabınız aktif değil. Lütfen e-postanızı kontrol ediniz.");
                }
            }
            else
            {
                blResult.Errors.Add("Kullanıcı adı ya da şifre hatalı. Ya da kayıtlı kullanıcı değilsiniz.");
            }
            return blResult;
        }

        public BusinessLayerResult<BlogUser> RegisterUser(RegisterViewModel model) //kullanıcıdan veri alacağız
        {
            BlogUser user = Find(x=> x.UserName == model.UserName || x.Email == model.Email);
            BusinessLayerResult<BlogUser> layerResult = new BusinessLayerResult<BlogUser>();
            if(user != null)
            {
                //kullanıcı adı ve email : bunlardan hangisi sistemde var
                if(user.UserName == model.UserName)
                {
                    layerResult.Errors.Add("Kullanıcı adı sistemde kayıtlı.");
                }
                if(user.Email == model.Email)
                {
                    layerResult.Errors.Add("Girdiğiniz E-posta sistemde kayıtlı.");
                }
            }
            else
            {
                //veritabanına bu kullanıcı kaydedilecek.
                int result = base.Insert(new BlogUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    UserProfileImage = "user-profile.png",
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    IsActive = false,
                    IsAdmin = false,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ModifiedUserName = model.UserName,
                    ActivateGuid = Guid.NewGuid()
                });
                if(result > 0)
                {
                    layerResult.Result = Find(x=> x.UserName == model.UserName && x.Email == model.Email);

                    //kullanıcıya aktivasyon maili göndermek için gerekli kodları buraya yazacağım.

                    //web.config dosyasına sitemizin root'unu eklemiştik. bu bilgiyi alıyoruz
                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                    //gönderilen maildeki active linkini oluşturmak için activateUrl değişkenini tanımladık.
                    // root/controlller/action/guidParametresi ile link oluşturuldu.
                    string activateUrl = $"{siteUrl}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    //mailde göndereceğimiz mesajın içeriğini aşağıda oluşturdum. html tagleri kullandım. mailHelper class'ında isHtml=true olduğu için html taglerini kullanabiliyorum.
                    string messageBody = $"Merhaba, hesabınızı aktifleştirmek için <a href = '{activateUrl}' target='_blank'> tıklayınız</a>";
                    //mail için konu
                    string subject = "NA-203 Blog Hesap Aktifleştirme";
                    //mailHelper'ın SendMail metoduna yukarıdaki parametreleri veriyorum. gönnderilecek kişinin mailini yukarıda sorguladığım layerResult.Result içinden alıyorum.
                    MailHelper.SendMail(messageBody, layerResult.Result.Email, subject);

                }
            }
            return layerResult;
        }

        public BusinessLayerResult<BlogUser> UpdateProfile(BlogUser userData)
        {
            BusinessLayerResult<BlogUser> blResult = new BusinessLayerResult<BlogUser>();
            BlogUser userDb = Find(x=> x.Id != userData.Id && (x.Email == userData.Email || x.UserName == userData.UserName));
            if(userDb != null && userDb.Id != userData.Id)
            {
                if(userDb.UserName == userData.UserName)
                {
                    blResult.Errors.Add("Girdiğiniz kullanıcı adı başka bir üyemiz tarafından kullanılmaktadır. Lütfen farklı bir kullanıcı adı giriniz.");
                }
                if(userDb.Email == userData.Email)
                {
                    blResult.Errors.Add("Girdiğiniz E-posta adresi sistemde kayıtlıdır. Lütfen farklı bir E-posta adresi giriniz.");
                }
                return blResult;
            }
            //eğer herhangi bir hata yoksa o zaman if bloğuna girmeyecek ve buradan devam edecek. Bu satırdan sonra Update işlemlerini yapmam gerekecek.
            blResult.Result = Find(x=> x.Id == userData.Id);
            blResult.Result.Name = userData.Name;
            blResult.Result.Surname = userData.Surname;
            blResult.Result.Email = userData.Email;
            blResult.Result.UserName = userData.UserName;
            blResult.Result.Password = userData.Password;
            //fotoğraf geldiyse bunun kontrolünü yapıyorum;
            if(string.IsNullOrEmpty(userData.UserProfileImage) == false)
            {
                blResult.Result.UserProfileImage = userData.UserProfileImage;
            }
            if(base.Update(blResult.Result) == 0)
            {
                blResult.Errors.Add("Profil güncellenemedi.");
            }
            return blResult;
        }

        public BusinessLayerResult<BlogUser> UserActivate(Guid id)
        {
            BusinessLayerResult<BlogUser> blResult = new BusinessLayerResult<BlogUser>();
            blResult.Result = Find(x=> x.ActivateGuid == id);
            if(blResult.Result != null)
            {
                if(blResult.Result.IsActive)
                {
                    blResult.Errors.Add("Kullanıcı zaten aktif edilmiştir.");
                }
                else
                {
                    blResult.Result.IsActive = true;
                    Update(blResult.Result);
                }
            }
            else
            {
                blResult.Errors.Add("Aktifleştirilecek kullanıcı bulunamadı.");
            }
            return blResult;
        }
        //method hiding
        //miras olarak gelen bir metodu ezmek istiyorsam ve geri dönüş tipini değiştirmek istiyorsam;
        //BaseManager içindeki insert isimli metodu ezmek istiyoruz. BaseManager içindeki insert metodunun geri dönüş tipi int türünde. fakat ben farklı bir türün geriye dönmesini istiyorsam (örneğin string ya da <blogUser>), bu durumda aşağıdaki gibi new keyword'ünü kullanarak ezmek istediğim metodun geri dönüş tipini değiştirebilirim ve artık insert metodu kullanılmak istendiğinde buradaki metot kullanılacak.
        //yukarıda RegisterUser metodunda BaseManager'daki insert metodunu kullanmak istediğimizden orada insert metodunun önüne base'i ekledik: base.Insert(baseaManager'daki insert metodu.)
        public new BusinessLayerResult<BlogUser> Insert(BlogUser data)
        {
            BlogUser user = Find(x=> x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<BlogUser> layerResult = new BusinessLayerResult<BlogUser>();
            layerResult.Result = data;
            if(user != null)
            {
                //bu durumda bir hata olmalı. Yani email ve kullanıcı adı başka bir kullanıcı tarafından kullanılıyor. kaydetme işlemi yapılmamalı ve geriye de kaydı giren kişiye uyarı mesajları gönderilmeli.
                if(user.Email == data.Email)
                {
                    layerResult.Errors.Add("E-posta adresi kayıtlı.");
                }
                if (user.UserName == data.UserName)
                {
                    layerResult.Errors.Add("Kullanıcı adı kayıtlı.");
                }
            }
            else
            {
                //username ve email ile eşleşen kayıt yok ise veriyi ekleme işlemini yapmamız gerekiyor.
                layerResult.Result.UserProfileImage = "user-profile.png";
                layerResult.Result.ActivateGuid = Guid.NewGuid();
                if(base.Insert(layerResult.Result) == 0)
                {
                    layerResult.Errors.Add("Yeni üye kaydedilirken bir hata oluştu.");
                }
            }
            return layerResult;
        }

        public new BusinessLayerResult<BlogUser> Update (BlogUser data)
        {
            BusinessLayerResult<BlogUser> layerResult = new BusinessLayerResult<BlogUser>();
            BlogUser dbUser = Find(x=>x.Id != data.Id && (x.Email == data.Email || x.UserName == data.UserName));
            layerResult.Result = data;
            if(dbUser != null && dbUser.Id != data.Id)
            {
                if(dbUser.UserName == data.UserName)
                {
                    layerResult.Errors.Add("Girdiğiniz kullanıcı adı başka bir üyemiz tarafından kullanılıyor. Lütfen farklı bir kullanıcı adı girin.");
                }
                if (dbUser.Email == data.Email)
                {
                    layerResult.Errors.Add("Girdiğiniz E-posta başka bir üyemiz tarafından kullanılıyor. Lütfen farklı bir E-posta girin.");
                }
                return layerResult;
            }
            //eğer hata yoksa update ile ilgili işlemleri yapmalıyız.
            layerResult.Result = Find(x => x.Id == data.Id);
            layerResult.Result.Email = data.Email;
            layerResult.Result.Name = data.Name;
            layerResult.Result.Surname = data.Surname;
            layerResult.Result.Password = data.Password;
            layerResult.Result.UserName = data.UserName;
            layerResult.Result.IsActive = data.IsActive;
            layerResult.Result.IsAdmin = data.IsAdmin;
            if(base.Update(layerResult.Result) == 0)
            {
                layerResult.Errors.Add("Profil güncellenirken bir hata oluştu.");
            }
            return layerResult;
        }
    }
}
