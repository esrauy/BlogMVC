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
    [Table("Categories")]
    public class Category : BaseEntity
    {
        [Required, DisplayName("Kategori Adı"), StringLength(50)]
        public string Title { get; set; }
        [StringLength(200), DisplayName("Açıklama")]
        public string Description { get; set; }
       
        //ilişkili alanlar
        //her kategorinin birden fazla notu olabilir
        public virtual List<Note> Notes { get; set; }

        public Category()
        {
            Notes = new List<Note>();  //fakeData ile data oluştururken hata vermesini önlemek için bu satırı ekliyoruz.
        }

    }
}
