using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class Item
    {
        [DisplayName("商品編號")]
        public int Id { get; set; }

        [DisplayName("商品名稱")]
        [Required(ErrorMessage = "請輸入商品名稱")]
        [StringLength(20, ErrorMessage = "商品名稱不可大於20字元")]
        public string Name { get; set; }

        [DisplayName("價格")]
        [Required(ErrorMessage = "請輸入價格")]
        [Range(typeof(int), "1", "9999", ErrorMessage = "價格請介於1-9999")]
        public int Price { get; set; }

        [DisplayName("圖片")]
        public string Image { get; set; }

        [DisplayName("商品描述")]
        [Required(ErrorMessage = "請輸入商品描述")]
        [StringLength(500, ErrorMessage = "商品描述不可大於500字元")]
        public string Description { get; set; }

        [DisplayName("類別")]
        [Required(ErrorMessage ="請選擇商品類別")]
        public int Catalog { get; set; }

    }
}