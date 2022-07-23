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

        public IEnumerable<Order> GetList()
        {
            salesManagementContext = new SalesManagementContext();
            return salesManagementContext.Orders.Include(o => o.OrderDetails).Include(order => order.Member).ToList();
        }
        public IEnumerable<Order> GetListIgnore()
        {
            salesManagementContext = new();
            return salesManagementContext.Orders.IgnoreAutoIncludes().ToList();
        }
        public Order? GetById(int orderId)
        {
            salesManagementContext = new();  
            return salesManagementContext.Orders.Include(order => order.OrderDetails).Include(order => order.Member).SingleOrDefault(o => o.OrderId.Equals(orderId));
        }

        public Order Add(Order order)
        {
            salesManagementContext = new SalesManagementContext();
            try
            {
                salesManagementContext.Orders.Add(order);
                salesManagementContext.SaveChanges();
                salesManagementContext.Dispose();
            } catch (Exception ex)
            {
                salesManagementContext.Orders.Remove(order);
                salesManagementContext.Dispose();
                throw ex;
            }
            return order;
        }

        public void Delete(Order order)
        {
            salesManagementContext = new SalesManagementContext();
            salesManagementContext.Orders.Remove(order);
            salesManagementContext.SaveChanges();
            salesManagementContext.Dispose();
        }

        public void Update(Order order)
        {
            salesManagementContext = new SalesManagementContext();
            salesManagementContext.Update<Order>(order);
            salesManagementContext.SaveChanges();
            salesManagementContext.Dispose();
        }

        public IEnumerable<Order> FilterByDate(DateTime startDate, DateTime endate)
        {
            salesManagementContext ??= new();
            return salesManagementContext.Orders.Where(o => (o.OrderDate.Value.CompareTo(startDate.Date) >= 0 && o.OrderDate.Value.CompareTo(endate.Date) <= 0)).ToList().OrderByDescending(o => o.OrderDate);
        }

        public IEnumerable<Order> SortDescByDate()
        {
            salesManagementContext ??= new();
            return salesManagementContext.Orders.ToList().OrderByDescending(o => o.OrderDate).ToList();
        }

        public IEnumerable<Order> GetByMemberId(int memberId)
        {
            salesManagementContext = new();
            return salesManagementContext.Orders.Where(order => order.MemberId == memberId).Include(order => order.OrderDetails).ToList();
        }
        public IEnumerable<OrderDetail> GetOrderDetails(int orderId)
        {
            salesManagementContext = new();
            return salesManagementContext.OrderDetails.Where(orderDetail => orderDetail.OrderId == orderId).Include(orderDetail => orderDetail.Product).ToList();
        }
    }
}
