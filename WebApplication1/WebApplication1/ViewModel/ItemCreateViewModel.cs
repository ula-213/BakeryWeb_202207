using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class ItemCreateViewModel
    {
        [DisplayName("商品圖片")]
        [FileExtensions(ErrorMessage = "所上傳檔案非圖檔")]
        [Required(ErrorMessage ="請上傳商品照片")]
        public HttpPostedFileBase ItemImage { get; set; }
        public Item NewData { get; set; }
    }
}