using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Service;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService orderService = new OrderService();
        public ActionResult Index()
        {
            return View();
        }
        #region 取得最近50筆訂單
        [Authorize(Roles ="Admin")]
        public ActionResult GetLastest50Order()
        {
            OrderViewModel data = new OrderViewModel();
            data.orderList = orderService.GetLastest50Order();
            return View(data);
        }
        #endregion
        #region 取得單筆訂單購買內容
        [Authorize(Roles ="Admin")]
        public ActionResult GetOrderItem(string Cart_Id)
        {
            OrderViewModel data = new OrderViewModel();
            data.orderItemList = orderService.GetOrderItem(Cart_Id);
            ViewData["Cart_Id"] = Cart_Id;
            return View(data);
        }
        #endregion
    }
}