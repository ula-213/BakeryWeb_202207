using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Order
    {
        public string Account { get; set; }
        public string Cart_Id { get; set; }
        [DisplayName("收件人姓名")]
        [Required(ErrorMessage ="請輸入收件人姓名")]
        [StringLength(20, MinimumLength =1, ErrorMessage = "收件人姓名需介於1-20字元")]
        public string Name { get; set; }
        [DisplayName("收件地址")]
        [Required(ErrorMessage ="請輸入收件地址")]
        [StringLength(200, MinimumLength =5, ErrorMessage ="請填寫正確地址")]
        public string Adr { get; set; }
        [DisplayName("郵遞區號")]
        [Required(ErrorMessage ="請輸入郵遞區號")]
        [Range(typeof(int), "100", "99999", ErrorMessage ="請輸入正確郵遞區號")]
        public int PostCode { get; set; }
    }
}