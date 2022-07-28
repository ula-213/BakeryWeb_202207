using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class OrderViewModel
    {
        public List<Order> orderList { get; set; }
        public Item item { get; set; }
        public CartSave cartsave { get; set; }
        public List<OrderItem> orderItemList { get; set; }
    }
}