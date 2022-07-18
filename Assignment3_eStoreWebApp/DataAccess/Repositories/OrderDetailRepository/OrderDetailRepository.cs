using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void Add(OrderDetail orderDetail) => OrderDetailDAO.Instance.Add(orderDetail);
        public void Delete(OrderDetail orderDetail) => OrderDetailDAO.Instance.Delete(orderDetail);
    }
}
