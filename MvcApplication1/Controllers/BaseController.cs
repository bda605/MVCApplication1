using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
namespace MvcApplication1.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        protected MvcShoppingContext db = new MvcShoppingContext();
        protected List<Cart> Carts
        {
            get
            {
                if (Session["Carts"] == null)
                {
                    Session["Carts"] = new List<Cart>();
                }
                return (Session["Carts"] as List<Cart>);
            }
            set
            {
                Session["Carts"] = value;
            }
        }
        

    }
}
