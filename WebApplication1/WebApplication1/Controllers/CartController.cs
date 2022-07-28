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
            string Cart = cartService.GetCartSave(User.Identity.Name);
            if (Cart != null)
            {
                HttpContext.Session["Cart"] = Cart;
            }

            CartBuyViewModel data = new CartBuyViewModel();
            
            data.DataList = cartService.GetItemFromCart(Cart);
            data.isCartSave = cartService.CheckCartSave(User.Identity.Name, Cart);
            return View(data);
        }
        #region 將商品放入購物車
        [Authorize]
        public ActionResult Put(int Id, string ToPage, int qty)
        {
            //將商品放入購物車時，若無購物車則建立新購物車
            if(HttpContext.Session["Cart"] == null)
            {
                DateTime GetNowDateTimeDetail = new DateTime(0001, 01, 01, 01, 01, 01, 01);
                GetNowDateTimeDetail = DateTime.Now;
                string strTime = GetNowDateTimeDetail.ToString("yyyy-MM-dd hh:mm:ss.fff");

                HttpContext.Session["Cart"] = User.Identity.Name + strTime;
                if(!cartService.CheckCartSave(User.Identity.Name, HttpContext.Session["Cart"].ToString()))
                {
                    cartService.SaveCart(User.Identity.Name, HttpContext.Session["Cart"].ToString());

                }
            }

            //如果購物車內有同種類商品
            if (cartService.CheckCartItem(HttpContext.Session["Cart"].ToString(), Id))
            {
                cartService.UpdateQuantityFromCart(HttpContext.Session["Cart"].ToString(), Id, qty);
            }
            else
            {
                cartService.AddtoCart(HttpContext.Session["Cart"].ToString(), Id, qty);
            }
            
            if (ToPage == "Item")
            {
                return RedirectToAction("Index", "Item");
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
        #region 結帳
        [Authorize]
        public ActionResult Checkout(int Total)
        {
            ViewData["Total"] = Total;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Checkout(CartBuyViewModel order)
        {
            order.Order1.Account = User.Identity.Name;
            order.Order1.Cart_Id = HttpContext.Session["Cart"].ToString();

            DateTime GetNowDateTimeDetail = new DateTime(0001, 01, 01);
            GetNowDateTimeDetail = DateTime.Now;
            string strTime = GetNowDateTimeDetail.ToString("yyyy-MM-dd");

            string validteStr = cartService.GenerateOrder(order, strTime);
            if (validteStr == "訂單完成")
            {              
                cartService.SetCartFinished(order.Order1.Account, order.Order1.Cart_Id);
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Item");

            }
            return View(order);

        }
        #endregion
    }
}