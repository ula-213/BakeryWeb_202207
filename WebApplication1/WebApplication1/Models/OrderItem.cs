using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OrderItem
    {

        [DisplayName("圖片")]
        public string Image { get; set; }
        [DisplayName("品名")]
        public string Name { get; set; }
        [DisplayName("數量")]
        public int Quantity { get; set; }
        [DisplayName("單價")]
        public int Price { get; set; }


    }
}