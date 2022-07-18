using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<OrderDetail> GetListByOrderId(int orderId) => salesManagementContext.OrderDetails.Where(o => o.OrderId.Equals(orderId)).ToList();
        public void Add(OrderDetail od)
        {
            salesManagementContext.OrderDetails.Add(od);
            salesManagementContext.SaveChanges();
        }
        public void Delete(OrderDetail od)
        {
            salesManagementContext.OrderDetails.Remove(od);
            salesManagementContext.SaveChanges();
        }
    }
}
