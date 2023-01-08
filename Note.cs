using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites
{
    [Table("Notes")]
    public class Note : BaseEntity
    {
        [Required, StringLength(50), DisplayName("Başlık")]
        public string Title { get; set; }
        [Required, StringLength(2000), DisplayName("Metin")]
        public string Text { get; set; }
        [DisplayName("Taslak Mı")]
        public bool IsDraft { get; set; }
        [DisplayName("Beğeni Sayısı")]
        public int LikeCount { get; set; }
        [DisplayName("Kategori Id")]
        public int CategoryId { get; set; }

        //ilişkiler
        //her notun ait olduğu bir kategori vardır
        public virtual Category Category { get; set; }
        //her notunait olduğu bir kullanıcısı olacaktır
        public virtual BlogUser Owner { get; set; }
        //birden fazla yorumu olabilir
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

        public Note()
        {
            //fakeData oluştururken hata verecek, bunun önüne geçmek için bu satırları ekliyorum.
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}
