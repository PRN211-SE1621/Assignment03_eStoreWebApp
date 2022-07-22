using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObject
{
    public partial class Member
    {
        public Member()
        {
            Orders = new HashSet<Order>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Member ID")]
        public int MemberId { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
