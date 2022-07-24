using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class Members
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage ="請輸入帳號")]
        [StringLength(30, MinimumLength =6, ErrorMessage ="帳號需介於6-30字元")]
        [Remote("AccountCheck", "Members", ErrorMessage ="帳號已被註冊")]
        public string Account { get; set; }
        public string Password { get; set; }
        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(30, ErrorMessage = "姓名長度做多30字元")]
        public string Name { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "請輸入Email")]
        [EmailAddress(ErrorMessage ="非Email格式")]
        public string Email { get; set; } 
        public string AuthCode { get; set; }
        public bool IsAdmin { get; set; }
    }
}