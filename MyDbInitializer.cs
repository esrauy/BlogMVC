using Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_DataAccessLayer.EntityFrameworkSQL
{
    public class MyDbInitializer : CreateDatabaseIfNotExists<BlogContext>
    {
        //FakeData DLL'ini ekledikten sonra bu class'ı oluşturuyorum.
        //bu class, database ilk oluştuğu anda ilk verilerin yani geliştirme aşamasında kullanacağımız test verilerinin oluşturulup database'e yüklenmesi amacı ile oluşturulmuştur.
        //bu class'ın ne zaman çalışacağına ilişkin bilgiyi miras yoluyla verdiğimiz farklı bir class belirleyecek.
        //CreateDatabaseIfNotExist<> : Database yoksa çalışır.
        //DropCreateDatabaseAlways: her çalıştığında database'i sil ve yeniden yarat.
        //DropCreateDatabaseIfModelChanges: eğer herhangi bir tablo değişirse database'i sil ve yeniden yarat.
        protected override void Seed(BlogContext context)
        {
            // Öncelikle 2 tane kullanıcı yaratalım.
            BlogUser admin = new BlogUser()
            {
                Name = "Admin",
                Surname = "Admin",
                UserProfileImage = "user-profile.png",
                Email = "admin@admin.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "admin",
                Password = "123456",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ModifiedUserName = "admin"
            };

            BlogUser standartUser = new BlogUser()
            {
                Name = "Mustafa",
                Surname = "Kavusdu",
                UserProfileImage = "user-profile.png",
                Email = "mkavusdu@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "mkavusdu",
                Password = "123456",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "mkavusdu"
            };

            context.BlogUsers.Add(admin);
            context.BlogUsers.Add(standartUser);

            for (int i = 0; i < 10; i++)
            {
                BlogUser user = new BlogUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    UserProfileImage = "user-profile.png",
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user-{i}",
                    Password = "123",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now.AddMinutes(5),
                    ModifiedUserName = $"user-{i}"
                };
                context.BlogUsers.Add(user);
            }
            context.SaveChanges();

            // Kullanıcı listesini database'ten alıyorum. Note ve Comment gibi tablolarda da kullanacağım.
            List<BlogUser> userList = context.BlogUsers.ToList();


            // Fake kategori eklenecek
            for (int i = 0; i < 10; i++)
            {
                Category category = new Category()
                {
                    Title = FakeData.PlaceData.GetCountry(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now.AddMinutes(5),
                    ModifiedUserName = "mkavusdu"
                };

                context.Categories.Add(category);

                // Fake note ekliyorum
                for (int j = 0; j < FakeData.NumberData.GetNumber(3, 15); j++)
                {
                    BlogUser user_note = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.PlaceData.GetCity(),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 4)),
                        Category = category,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 12),
                        CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                        ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                        ModifiedUserName = user_note.UserName,
                        Owner = user_note
                    };

                    category.Notes.Add(note);

                    //Comment Eklemek için aşağıdaki kodları yazıyorum

                    for (int k = 0; k < FakeData.NumberData.GetNumber(5, 15); k++)
                    {
                        BlogUser commentuser = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = commentuser,
                            CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                            ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                            ModifiedUserName = commentuser.UserName

                        };
                        note.Comments.Add(comment);
                    }

                    // Fake Like datası ekliyorum

                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = user_note,
                        };
                        note.Likes.Add(liked);
                    }
                }

            }
            context.SaveChanges();
        }
    }
}
