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

        public IEnumerable<Order> GetList() => salesManagementContext.Orders.Include(o => o.OrderDetails).ToList();
        public IEnumerable<Order> GetListIgnore() => salesManagementContext.Orders.IgnoreAutoIncludes().ToList();
        public IEnumerable<Order> GetListByMemberId(int memberId) => salesManagementContext.Orders.Where(o => o.Member.MemberId.Equals(memberId));

        public Order? GetById(int orderId) => salesManagementContext.Orders.SingleOrDefault(o => o.OrderId.Equals(orderId));

        public void Add(Order order)
        {
            salesManagementContext.Orders.Add(order);
            salesManagementContext.SaveChanges();
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
            => salesManagementContext.Orders.Where(o => (o.OrderDate.Date.CompareTo(startDate.Date) >= 0 && o.OrderDate.Date.CompareTo(endate.Date) <= 0)).ToList().OrderByDescending(o => o.OrderDate);

        public IEnumerable<Order> SortDescByDate()
            => salesManagementContext.Orders.ToList().OrderByDescending(o => o.OrderDate);

        public IEnumerable<Order> GetByMemberId(int memberId) => salesManagementContext.Orders.Where(o => o.MemberId == memberId).Include(o => o.OrderDetails).ToList();

    }
}
