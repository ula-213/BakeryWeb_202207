using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class RegisterViewModel
    {
        public Members Member { get; set; }
        [DisplayName("密碼")]
        [Required(ErrorMessage ="請輸入密碼")]
        public string Password { get; set; }
        [DisplayName("密碼確認")]
        [Required(ErrorMessage = "請輸入密碼確認")]
        [Compare("Password", ErrorMessage ="兩次密碼輸入不一致")]
        public string PasswordCheck { get; set; }

    }
}