using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Entites.ViewModels
{
    public class RegisterViewModel
    {
        [DisplayName("Adınız"), Required(ErrorMessage = "{0} boş geçilemez."), StringLength(25, ErrorMessage = "{0} alanı en fazla 25 karakter olmalı.")]
        public string Name { get; set; }
        [DisplayName("Soyadınız"), Required(ErrorMessage = "{0} boş geçilemez."), StringLength(25, ErrorMessage = "{0} alanı en fazla 25 karakter olmalı.")]
        public string Surname { get; set; }

        [DisplayName("E-posta"), Required(ErrorMessage = "{0} boş geçilemez."),StringLength(50, ErrorMessage = "{0} alanı en fazla 50 karakter olmalıdır.") , EmailAddress(ErrorMessage = "{0} Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }

        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} boş geçilemez."), StringLength(25, ErrorMessage = "{0} alanı en fazla 25 karakter olmalıdır.")]
        public string UserName { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "Şifre boş geçilemez."), DataType(DataType.Password), StringLength(25, ErrorMessage = "Şifre alanı en fazla 25 karakter olmalıdır.")]
        public string Password { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = " Şifre tekrar alanı boş geçilemez."), DataType(DataType.Password), StringLength(25, ErrorMessage = "Şifre alanı en fazla 25 karakter olmalıdır."), Compare("Password", ErrorMessage = "Girilen şifreler eşleşmiyor.")]
        public string RePassword { get; set; }

    }
}