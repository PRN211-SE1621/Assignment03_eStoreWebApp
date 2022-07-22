using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.Common
{
    public class Helper
    {
        public static bool CheckRole(Member member)
        {
            if (member == null)
            {
                return false;
            }
            return member.Email.Equals("admin@fstore.com");           
        }
    }
}
