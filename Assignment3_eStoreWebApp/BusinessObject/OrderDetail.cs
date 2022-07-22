using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class OrderDetail
    {
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Discount")]
        public double Discount { get; set; }
        [Display(Name = "Order")]
        public virtual Order Order { get; set; }
        [Display(Name = "Product")]
        public virtual Product Product { get; set; }
    }
}
