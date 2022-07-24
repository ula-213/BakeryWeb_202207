using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CartBuy
    {
        //購物車編號
        public string Cart_Id { get; set; }
        //商品編號
        public int Item_Id { get; set; }
        //單一商品購買數量
        public int Quantity { get; set; }
        //item資料表
        public Item Item { get; set; } = new Item();
    }
}