using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Order
    {
        public string Account { get; set; }
        public string Cart_Id { get; set; }
        [DisplayName("收件人姓名")]
        public string Name { get; set; }
        [DisplayName("收件地址")]
        public string Adr { get; set; }
        [DisplayName("郵遞區號")]
        public int PostCode { get; set; }
    }
}