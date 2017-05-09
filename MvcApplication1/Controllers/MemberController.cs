using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcApplication1.Models;
namespace MvcApplication1.Controllers
{
    public class MemberController : BaseController
    {
        MvcShoppingContext db = new MvcShoppingContext();
        //
        // GET: /Member/
        public ActionResult Register() 
        {
            return View();
        }
        private string pwSalt = "AlrySqloPe2Mh784QQwG6jRAfkdPpDa90J0i";
        [HttpPost]
        public ActionResult Register([Bind(Exclude="RegisterOn,AuthCode")] Member member) 
        {
            var chk_member = db.Members.Where(p => p.Email == member.Email).FirstOrDefault();
            if (chk_member != null) 
            {
                ModelState.AddModelError("Email", "您輸入Email有人註冊過了");
            }
            if (ModelState.IsValid)
            {
                member.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(pwSalt + member.Password, "SHA1");
                member.RegisterOn = DateTime.Now;
                member.AuthCode = Guid.NewGuid().ToString();
                db.Members.Add(member);
                db.SaveChanges();

                SendAuthCodeToMember(member);
                return RedirectToAction("Index", "Home");
            }
            else {
                return View();
            }
 
        }
        private void SendAuthCodeToMember(Member member) 
        {
           
        }
        public ActionResult Login(string returnUrl) 
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string returnUrl) 
        {
            if(ValidateUser(email,password))
            {
                FormsAuthentication.SetAuthCookie(email,false);
                if(string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index","Home");
                }else{
                    return Redirect(returnUrl);
                }
            }
            ModelState.AddModelError("", "你輸入帳號或密碼有錯誤");
            return View();
        }

        public bool ValidateUser(string email, string password) 
        {
            var hash_pw = FormsAuthentication.HashPasswordForStoringInConfigFile(pwSalt + password, "SHA1");
            var member = (from p in db.Members
                          where p.Email == email && p.Password == hash_pw
                          select p).FirstOrDefault();
            return (member != null);
        }
        public ActionResult Logout() 
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
	}
}