using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
namespace MvcApplication1.Controllers
{
    public class CartController : BaseController
    {
       
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return View(this.Carts);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, int amount = 1) 
        {
            var product = db.Products.Find(productId);
            if (product == null)
                return HttpNotFound();
            var existingCart = this.Carts.FirstOrDefault(p => p.Product.Id == productId);
            if (existingCart != null)
            {
                existingCart.Amount += 1;
            }
            else {
                this.Carts.Add(new Cart() { Product = product, Amount = amount });
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
        }

        [HttpPost]
        public ActionResult Remove(int productId) 
        {
            var existingCart = this.Carts.FirstOrDefault(p => p.Product.Id == productId);
            if (existingCart != null)
                this.Carts.Remove(existingCart);

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
           
        }
        [HttpPost]
        public ActionResult UpdateAmount(List<Cart> Carts)
        {
            foreach (var item in Carts) 
            {
                var existingCart = this.Carts.FirstOrDefault(p => p.Product.Id == item.Product.Id);
                if (existingCart != null) 
                {
                    existingCart.Amount = item.Amount;
                }
            }
            return RedirectToAction("Index", "Cart");

        }
	}
}