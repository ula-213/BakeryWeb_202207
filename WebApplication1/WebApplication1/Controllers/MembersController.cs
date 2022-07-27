using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.ViewModel;
using WebApplication1.Service;
using WebApplication1.Security;
using System.Web.Configuration;

namespace WebApplication1.Controllers
{
    public class MembersController : BaseController
    {
        private readonly MembersDBService memberService = new MembersDBService();
        private readonly MailService mailService = new MailService();
        private readonly CartService cartService = new CartService();
        
        public ActionResult Index()
        {
            return View();
        }
        #region 註冊

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ImgCarousel");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel newMember)
        {
            if (ModelState.IsValid)
            {
                newMember.Member.Password = newMember.Password;
                string AuthCode = mailService.GetValidateCode();
                newMember.Member.AuthCode = AuthCode;
                memberService.Register(newMember.Member);

                string TempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));

                UriBuilder ValidateUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("EmailValidate", "Members",
                    new
                    {
                        Account = newMember.Member.Account,
                        AuthCode = AuthCode
                    })
                };
                string MailBodey = mailService.GetRegisterMailBody(TempMail, newMember.Member.Account, ValidateUrl.ToString().Replace("%3F", "?"));
                mailService.SendRegisterMailBody(MailBodey, newMember.Member.Email);
                TempData["RegisterState"] = "註冊成功，請前往收信";
                return RedirectToAction("RegisterResult");

            }
            newMember.Password = null;
            newMember.PasswordCheck = null;
            return View(newMember);
        }
        #endregion
        #region 註冊結果       
        public ActionResult RegisterResult()
        {
            return View();
        }
        #endregion
        #region 接收驗證信
        public ActionResult EmailValidate(string Account, string AuthCode)
        {
            ViewData["EmailValidate"] = memberService.EmailValidate(Account, AuthCode);
            return View();
        }
        #endregion
        #region 帳號重複註冊
        public JsonResult AccountCheck(RegisterViewModel newMember)
        {
            return Json(memberService.AccountCheck(newMember.Member.Account), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 登入
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ImgCarousel");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel LoginMember)
        {
            string ValidateStr = memberService.LoginCheck(LoginMember.Account, LoginMember.Password);
            if (string.IsNullOrEmpty(ValidateStr))
            {
                HttpContext.Session.Clear();
                string Cart = cartService.GetCartSave(LoginMember.Account);
                //登入後先取得先前購物車資料
                if (Cart != null)
                {
                    HttpContext.Session["Cart"] = Cart;
                }
               

                string RoleDate = memberService.GetRole(LoginMember.Account);
                
                JwtService jwtService = new JwtService();
                string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
                string Token = jwtService.GenerateToken(LoginMember.Account, RoleDate);
                //產生cookie
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Value = Server.UrlEncode(Token);
                //寫到用戶端
                Response.Cookies.Add(cookie);
                Response.Cookies[cookieName].Expires = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"]));

                return RedirectToAction("Index", "ImgCarousel");
            }
            else
            {
                ModelState.AddModelError("", ValidateStr);
                return View(LoginMember);
            }
        }
        #endregion
        #region 修改密碼
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel LoginMember)
        {
            if (ModelState.IsValid)
            {
                ViewData["ChangeState"] = memberService.ChangePassword(User.Identity.Name, LoginMember.Password, LoginMember.newPassword);
            }
            return View();
        }
        #endregion
        #region 登出
        [Authorize]
        public ActionResult Logout()
        {
            string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Values.Clear();
            Response.Cookies.Set(cookie);
            return RedirectToAction("Login");
        }
        #endregion

    }
}