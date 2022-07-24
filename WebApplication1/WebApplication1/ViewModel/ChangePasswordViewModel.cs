using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class ChangePasswordViewModel
    {
        [DisplayName("舊密碼")]
        [Required(ErrorMessage ="請輸入舊密碼")]
        public string Password { get; set; }
        [DisplayName("新密碼")]
        [Required(ErrorMessage = "請輸入新密碼")]
        public string newPassword { get; set; }
        [DisplayName("新密碼確認")]
        [Required(ErrorMessage = "請輸入新密碼確認")]
        [Compare("newPassword", ErrorMessage ="兩次密碼輸入不一致")]
        public string newPasswordCheck { get; set; }
    }
}