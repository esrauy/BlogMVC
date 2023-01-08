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
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        [Required, StringLength(300), DisplayName("Metin")]
        public string Text { get; set; }
        //ilişkiler
        //hangi yazıya yorum yapıldı?
        public virtual Note Note { get; set; }
        //yorumu kim yaptı?
        public virtual BlogUser Owner { get; set; }

    }
}
