using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcApplication1.Models;
using System.Net.Mail;
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
           string mailBody = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/MemberRegisterEMailTemplate.htm"));
            mailBody = mailBody.Replace("{{Name}}",member.Name);
            mailBody = mailBody.Replace("{{RegisterOn}}",member.RegisterOn.ToString("F"));
            var auth_url = new UriBuilder(Request.Url)
            {
              Path = Url.Action("ValidateRegister",new {id = member.AuthCode}),
              Query = ""
            
            };
            mailBody = mailBody.Replace("{{AUTH_URL}}",auth_url.ToString());
            try{
                  SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                  SmtpServer.Port = 587;
                  SmtpServer.Credentials = new System.Net.NetworkCredential("bda605@gmail.com","CcCc121219511");
                  SmtpServer.EnableSsl = true;
                  MailMessage  mail = new MailMessage();
                  mail.From = new MailAddress("bda605@gmail.com");
                  mail.To.Add(member.Email);
                  mail.Subject = "會員註冊信";
                  mail.Body = mailBody;
                  mail.IsBodyHtml = true;
                  SmtpServer.Send(mail);
            }catch(Exception ex)
            {
              throw ex;
            }

        }
        public ActionResult ValidateRegister(string id) 
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var member = db.Members.Where(p => p.AuthCode == id).FirstOrDefault();
            if (member != null) {
                TempData["LastTempMessage"] = "會員驗證成功，您現在可以登入網站";
                member.AuthCode = null;
                db.SaveChanges();
            } else {
                TempData["LastTempMessage"] = "查無此會員驗證碼，可能通過認證";
            
            }

            return RedirectToAction("Login", "Member");
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
            if (member != null) {
                if (member.AuthCode == null) {
                    return true;
                } else {
                    ModelState.AddModelError("", "你尚未通會員認證，請收信並點擊會員認證");
                    return false;
                }
            } else {
                ModelState.AddModelError("", "您輸入帳號密碼錯誤");
                return false;
            }
           
        }
        [HttpPost]
        public ActionResult CheckDup(string email) 
        {
            var member = db.Members.Where(p => p.Email == email).FirstOrDefault();
            if (member != null)
                return Json(false);
            else
                return Json(true);
        }
        public ActionResult Logout() 
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
	}
}

 