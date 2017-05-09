using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
namespace MvcApplication1.Controllers
{
    public class HomeController : BaseController
    {
        MvcShoppingContext db = new MvcShoppingContext();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //var data = new List<ProductCategory>()
            //{
            //    new ProductCategory(){Id=1  ,Name="文具"},
            //    new ProductCategory(){Id=2  ,Name = "禮品"},
            //    new ProductCategory() {Id=3 ,Name = "書籍"},
            //    new ProductCategory () {Id = 4 ,Name = "美勞用具"}
            //};
            var data = db.ProductCategories.ToList();
            if (data.Count == 0) 
            {
                db.ProductCategories.Add(new ProductCategory() { Id = 1, Name = "文具" });
                db.ProductCategories.Add(new ProductCategory() { Id = 2, Name = "禮品" });
                db.ProductCategories.Add(new ProductCategory() { Id = 3, Name = "書籍" });
                db.ProductCategories.Add(new ProductCategory() { Id = 4, Name = "美勞用具" });
                db.SaveChanges();
                data = db.ProductCategories.ToList();
            }
            return View(data);
        }

        public ActionResult ProdcutList(int id) 
        {
            //var productCategory = new ProductCategory() { Id = id, Name = "類別" + id };
            //var data = new List<Product>()
            //{
            //   new Product {Id = 1,ProductCategory = productCategory,Name = "原子筆",Description="N/A",Price=30,PublishOn = DateTime.Now,Color = Color.Black},
            //   new Product {Id = 2,ProductCategory = productCategory,Name = "鉛筆",Description="N/A",Price=5,PublishOn = DateTime.Now,Color = Color.Black},
 
            //};
            var productCategory = db.ProductCategories.Find(id);
            var data = productCategory.Products.ToList();

            if (data.Count == 0) 
            {
                 productCategory.Products.Add(new Product(){ Name=productCategory.Name +"類別商品1",Color = Color.Red,Description = "N/A",Price=99,PublishOn=DateTime.Now,ProductCategory = productCategory});
                 productCategory.Products.Add(new Product() { Name = productCategory.Name + "類別商品2", Color = Color.Blue, Description = "N/A", Price = 150, PublishOn = DateTime.Now, ProductCategory = productCategory });
            }
            return View(data);
        }

        public ActionResult ProductDetail(int id) 
        {
            //var productCategory = new ProductCategory() { Id = 1, Name = "文具" };
            //var data = new Product() { Id = id, ProductCategory = productCategory, Name = "商品" + id, Description = "N/A", Price = 30, PublishOn = DateTime.Now, Color = Color.Black };
            var data = db.Products.Find(id);
            return View(data);
        }
	}
}