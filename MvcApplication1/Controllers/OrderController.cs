using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using MvcApplication1.Controllers;
namespace MvcApplication1.Controllers
{
    public class OrderController : BaseController
    {
     
        public ActionResult Complete() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Complete(OrderHeader form) 
        {
            var member = db.Members.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            if (member == null) return RedirectToAction("Index", "Home");

            if(this.Carts.Count ==0 )   return RedirectToAction("Index", "Cart");
            OrderHeader oh = new OrderHeader() 
            {
               Member =member,
               ContactName = form.ContactName,
               ContactAddress = form.ContactAddress,
               ContactPhoneNo = form.ContactPhoneNo,
               BuyOn = DateTime.Now,
               Memo = form.Memo,
               OrderDetailItems = new List<OrderDetail>()
            };
            int total_price = 0;
            foreach (var item in this.Carts) 
            {
                var product = db.Products.Find(item.Product.Id);
                if (product == null) return RedirectToAction("Index", "Cart");
                total_price += item.Product.Price * item.Amount;
                oh.OrderDetailItems.Add(new OrderDetail() { Product = product, Price = product.Price, Amount = item.Amount });
            }
            oh.TotalPrice = total_price;
            db.Orders.Add(oh);
            db.SaveChanges();
            this.Carts.Clear();
            return RedirectToAction("Index", "Home");
        }
	}
}