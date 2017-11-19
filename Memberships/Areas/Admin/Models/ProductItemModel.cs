using Memberships.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class ProductItemModel
    {
        [DisplayName("Product ID")]
        public int ProductId { get; set; }
        [DisplayName("Item ID")]
        public int ItemId { get; set; }
        [DisplayName("Item Title")]
        public string ItemTitle { get; set; }
        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}