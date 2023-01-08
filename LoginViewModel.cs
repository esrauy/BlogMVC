using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Entites.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} boş geçilemez."), StringLength(25, ErrorMessage = "{0} alanı en fazla 25 karakter olmalıdır.")]
        public string UserName { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "Şifre boş geçilemez."), DataType(DataType.Password), StringLength(25, ErrorMessage = "Şifre alanı en fazla 25 karakter olmalıdır.")]
        public string Password { get; set; }
    }
}