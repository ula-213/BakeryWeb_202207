using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class ItemDetailViewModel
    {
        //商品
        public Item Data { get; set; }
        //商品是否在購物車中
        public bool InCart { get; set; }
        public CartBuy cartBuy { get; set; }
    }
}