using System;
using System.Collections.Generic;

#nullable enable

namespace BusinessObject
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        [Display(Name = "Member ID")]
        public int? MemberId { get; set; }
        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; }
        [Display(Name = "Required Date")]
        public DateTime? RequiredDate { get; set; }
        [Display(Name = "Shipped Date")]
        public DateTime? ShippedDate { get; set; }
        [Display(Name = "Freight")]
        public decimal? Freight { get; set; }
        [Display(Name = "Member")]
        public virtual Member? Member { get; set; }
        [Display(Name = "Order Details")]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
