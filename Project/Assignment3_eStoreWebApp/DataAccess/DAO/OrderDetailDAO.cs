using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        private SalesManagementContext salesManagementContext = new SalesManagementContext();

        public OrderDetailDAO()
        {
        }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }



        public IEnumerable<OrderDetail> GetListByOrderId(int orderId)
            => salesManagementContext.OrderDetails.Where(o => o.OrderId.Equals(orderId)).Include(o => o.Product).ToList();


        public void Add(OrderDetail od)
        {
            salesManagementContext = new SalesManagementContext();
            salesManagementContext.OrderDetails.Add(od);
            salesManagementContext.SaveChanges();
            salesManagementContext.Dispose();
            salesManagementContext = new();
        }
        public void Delete(OrderDetail od)
        {
            salesManagementContext = new SalesManagementContext();
         
            salesManagementContext.OrderDetails.Remove(od);
            salesManagementContext.SaveChanges();
            salesManagementContext = new();
        }

       
    }


}
