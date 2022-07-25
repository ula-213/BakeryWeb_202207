using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Service;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class CartController : BaseController
    {
        private readonly CartService cartService = new CartService();
        
        [Authorize]
        public ActionResult Index()
        {
            CartBuyViewModel data = new CartBuyViewModel();
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            data.DataList = cartService.GetItemFromCart(Cart);
            data.isCartSave = cartService.CheckCartSave(User.Identity.Name, Cart);
            return View(data);
        }
        #region 將商品放入購物車
        [Authorize]
        public ActionResult Put(int Id, string ToPage)
        {
            if(HttpContext.Session["Cart"] == null)
            {
                DateTime GetNowDateTimeDetail = new DateTime(0001, 01, 01, 01, 01, 01, 01);
                GetNowDateTimeDetail = DateTime.Now;
                string strTime = GetNowDateTimeDetail.ToString("yyyy-MM-dd hh:mm:ss.fff");

                HttpContext.Session["Cart"] = User.Identity.Name + strTime;
            }
            cartService.AddtoCart(HttpContext.Session["Cart"].ToString(), Id);
            if (ToPage == "Item")
            {
                return RedirectToAction("Item", "Item", new { Id = Id });
            }
            else if(ToPage == "ItemBlock")
            {
                return RedirectToAction("Item", "Item", new { Id = Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        
        }
        #endregion
        #region 將商品取出購物車
        [Authorize]
        public ActionResult Pop(int Id, string ToPage)
        {
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            cartService.RemoveForCart(Cart, Id);
            if (ToPage == "Item")
            {
                return RedirectToAction("Item", "Item", new { Id = Id });
            }
            else if(ToPage == "ItemBlock")
            {
                return RedirectToAction("ItemBlock", "Item", new { Id = Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region 保存購物車
        [Authorize]
        public ActionResult CartSave()
        {
            string Cart;
            if (HttpContext.Session["Cart"] != null)
            {
                Cart = HttpContext.Session["Cart"].ToString();
            }
            else
            {
                DateTime GetNowDateTimeDetail = new DateTime(0001, 01, 01, 01, 01, 01, 01);
                GetNowDateTimeDetail = DateTime.Now;
                string strTime = GetNowDateTimeDetail.ToString("yyyy-MM-dd hh:mm:ss.fff");
                Cart = User.Identity.Name + strTime;
                HttpContext.Session["Cart"] = Cart;
            }
            cartService.SaveCart(User.Identity.Name, Cart);
            return RedirectToAction("Index");

        }
        #endregion
        #region 取消保存購物車
        [Authorize]
        public ActionResult CartSaveRemove()
        {
            cartService.SaveCartRemove(User.Identity.Name);
            return RedirectToAction("Index");
        }
        #endregion 
    }
}