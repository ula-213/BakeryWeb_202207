using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class CartBuyViewModel
    {
        public List<CartBuy> DataList { get; set; }
        public bool isCartSave { get; set; }
    }
}