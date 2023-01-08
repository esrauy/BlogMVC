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
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Güncellenme Tarihi")]
        public DateTime ModifiedDate { get; set; }
        [Required, StringLength(30),DisplayName("Güncelleyen Kullanıcı")]
        public string ModifiedUserName { get; set; }
    }
}
