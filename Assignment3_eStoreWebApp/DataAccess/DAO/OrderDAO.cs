using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private SalesManagementContext salesManagementContext = new SalesManagementContext();

        public OrderDAO()
        {
        }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Order> GetList() => salesManagementContext.Orders.Include(o => o.OrderDetails).Include(order => order.Member).ToList();
        public IEnumerable<Order> GetListIgnore() => salesManagementContext.Orders.IgnoreAutoIncludes().ToList();
        public IEnumerable<Order> GetListByMemberId(int memberId) => salesManagementContext.Orders.Where(o => o.Member.MemberId.Equals(memberId));

        public Order? GetById(int orderId) => salesManagementContext.Orders.Include(order => order.OrderDetails).Include(order => order.Member).SingleOrDefault(o => o.OrderId.Equals(orderId));

        public Order Add(Order order)
        {
            try
            {
                salesManagementContext.Orders.Add(order);
                salesManagementContext.SaveChanges();
            } catch (Exception ex)
            {
                salesManagementContext.Orders.Remove(order);
                throw ex;
            }
            return order;
        }

        public void Delete(Order order)
        {
            salesManagementContext.Orders.Remove(order);
            salesManagementContext.SaveChanges();
        }

        public void Update(Order order)
        {
            salesManagementContext.Update<Order>(order);
            //salesManagementContext.Orders.Update(order);
            salesManagementContext.SaveChanges();
        }

        public IEnumerable<Order> FilterByDate(DateTime startDate, DateTime endate)
            => salesManagementContext.Orders.Where(o => (o.OrderDate.Value.CompareTo(startDate.Date) >= 0 && o.OrderDate.Value.CompareTo(endate.Date) <= 0)).ToList().OrderByDescending(o => o.OrderDate);

        public IEnumerable<Order> SortDescByDate()
            => salesManagementContext.Orders.ToList().OrderByDescending(o => o.OrderDate);

        public IEnumerable<Order> GetByMemberId(int memberId) => salesManagementContext.Orders.Where(o => o.MemberId == memberId).Include(o => o.OrderDetails).ToList();
        public IEnumerable<OrderDetail> GetOrderDetails(int orderId) => salesManagementContext.OrderDetails.Where(orderDetail => orderDetail.OrderId == orderId).Include(orderDetail => orderDetail.Product);
    }
}
