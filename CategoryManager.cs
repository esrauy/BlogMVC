using Blog_DataAccessLayer.EntityFrameworkSQL;
using Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BusinessLayer
{
    public class CategoryManager : BaseManager<Category>
    {
        //private Repository<Category> repository = new Repository<Category>();

        //public List<Category> GetCategories()
        //{
        //    return repository.List();
        //}

        //public Category GetCategoryById(int id)
        //{
        //    return repository.Find(x=> x.Id == id);
        //}

        //delete metodunu normalde baseManager sınıfındaki metodu kullanıyor. biz BaseManager sınıfını abstract olarak işaretledik ve içindeki metotları da virtual olarak işaretledik. Bu sınıf (categoryManager) BaseManager sınıfını miras aldığı için ve yukarıda saydığımız özelliklerden dolayı, orada tanımlanan metotları burada ezebiliriz.
        public override int Delete(Category category)
        {
            //bir kategori silinebilmesi için ilişkili olan kayıtların da silinmesi gerekiyor (ilişkili alanlar: note, liked, comment)   

            NoteManager noteManager = new NoteManager();
            CommentManager commentManager = new CommentManager();
            LikedManager likedManager = new LikedManager();
            foreach(var note in category.Notes.ToList())
            {
                //bu note'a ait comment'leri de silmem gerekli.
                foreach(var comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }
                //bu note'a ait like'ları da silmem gerekli.
                foreach(var like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }
                noteManager.Delete(note);
            }

            //bu satır, bu metodun içindeki kodların yanında BaseManager içindeki delete metodunun da çalışacağı anlamına gelir.
            return base.Delete(category);
        }
    }
}
