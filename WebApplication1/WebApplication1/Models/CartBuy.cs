using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [DisplayName("數量")]
        [Range(1, 99, ErrorMessage = "數量需介於1-99")]
        public int Quantity { get; set; }
        //item資料表
        public Item Item { get; set; } = new Item();
    }
}