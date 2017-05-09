using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MvcApplication1.Models
{
    public class Cart
    {
        [DisplayName("選購商品")]
        [Required]
        public Product Product { get; set; }

        [DisplayName("選購數量")]
        [Required]
        public int Amount { get; set; }
    }
}