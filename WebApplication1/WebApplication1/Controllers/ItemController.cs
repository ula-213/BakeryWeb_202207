using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Service;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class ItemController : BaseController
    {
        private readonly CartService cartService = new CartService();
        private readonly ItemService itemService = new ItemService();
        
        public ActionResult Index(int Page = 1)
        {
            ItemViewModel Data = new ItemViewModel();
            Data.Paging = new ForPaging(Page);
            Data.IdList = itemService.GetIdList(Data.Paging);
            Data.ItemBlock = new List<ItemDetailViewModel>();
            foreach (var Id in Data.IdList)
            {
                ItemDetailViewModel newBlock = new ItemDetailViewModel();
                newBlock.Data = itemService.GetDataById(Id);
                //取得session內購物車資料
                string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
                //newBlock.InCart = cartService.CheckInCart(Cart, Id);
                Data.ItemBlock.Add(newBlock);
            }
            return View(Data);
        }
        #region 單一商品頁面
        public ActionResult Item(int Id)
        {
            ItemDetailViewModel ViewData = new ItemDetailViewModel();
            ViewData.Data = itemService.GetDataById(Id);
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            /*if (cartService.CheckCartItem(HttpContext.Session["Cart"].ToString(), Id))
            {
                cartService.UpdateQuantityFromCart(HttpContext.Session["Cart"].ToString(), Id, Quantity);
            }
            else
            {
                cartService.AddtoCart(HttpContext.Session["Cart"].ToString(), Id, Quantity);
            }*/
            return View(ViewData);

        }
        #endregion
        #region 商品列表區塊
        public ActionResult ItemBlock(int Id)
        {
            ItemDetailViewModel ViewData = new ItemDetailViewModel();
            ViewData.Data = itemService.GetDataById(Id);
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            //ViewData.InCart = cartService.CheckInCart(Cart, Id);
            return PartialView(ViewData);
        }
        #endregion
        #region 新增商品
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var productCatalogDropdownList = new List<SelectListItem>()
            {
                new SelectListItem() { Text="麵包系列", Value="1"},
                new SelectListItem() { Text = "蛋糕系列", Value="2"},
                new SelectListItem() {Text="喜餅系列", Value="3"}
            };
            productCatalogDropdownList.Where(q => q.Value == "1").First().Selected = true;
            ViewBag.selectList = productCatalogDropdownList;
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Add(ItemCreateViewModel Data)
        {
            if (Data.ItemImage != null)
            {
                string fileName = Path.GetFileName(Data.ItemImage.FileName);
                string url = Path.Combine(Server.MapPath("~/images/Product/"), fileName);
                Data.ItemImage.SaveAs(url);
                Data.NewData.Image = fileName;
                itemService.Insert(Data.NewData);
                return RedirectToAction("Create", "Item");
            }
            else
            {
                ModelState.AddModelError("ItemImage", " 請選擇上傳檔案");
                return View(Data);
            }
        }
        #endregion
        #region 商品類別
        public ActionResult CatalogItem(int Page = 1, int Catalog = 1)
        {
            ItemViewModel Data = new ItemViewModel();
            Data.Paging = new ForPaging(Page);
            Data.IdList = itemService.GetIdListByCatalog(Data.Paging, Catalog);
            Data.ItemBlock = new List<ItemDetailViewModel>();
            foreach (var Id in Data.IdList)
            {
                ItemDetailViewModel newBlock = new ItemDetailViewModel();
                newBlock.Data = itemService.GetDataById(Id);
                //取得session內購物車資料
                //string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
                //newBlock.InCart = cartService.CheckInCart(Cart, Id);
                Data.ItemBlock.Add(newBlock);
            }
            if (Catalog == 1)
            {
                ViewData["CatalogLabel"] = "麵包系列";
            }
            else if(Catalog == 2)
            {
                ViewData["CatalogLabel"] = "西點系列";
            }
            else if(Catalog == 3)
            {
                ViewData["CatalogLabel"] = "喜餅系列";
            }
            
            return View(Data);

        }
        #endregion
    }
}