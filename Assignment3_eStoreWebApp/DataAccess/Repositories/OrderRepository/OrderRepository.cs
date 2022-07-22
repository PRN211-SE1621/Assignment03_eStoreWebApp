using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public IEnumerable<Order> GetAll() => OrderDAO.Instance.GetList();
        public IEnumerable<Order> GetAllIgnore() => OrderDAO.Instance.GetListIgnore();
        public Order Add(Order order) => OrderDAO.Instance.Add(order);
        public void Delete(Order order) => OrderDAO.Instance.Delete(order);
        public void Update(Order order) => OrderDAO.Instance.Update(order);
        public Order? GetById(int id) => OrderDAO.Instance.GetById(id);
        public IEnumerable<Order> FilterByDate(DateTime start, DateTime end) => OrderDAO.Instance.FilterByDate(start, end);
        public IEnumerable<Order> SortDescByDate() => OrderDAO.Instance.SortDescByDate();
        public IEnumerable<Order> GetAllOfMember(int memberId) => OrderDAO.Instance.GetByMemberId(memberId);
        public IEnumerable<OrderDetail> GetOrderDetails(int orderId) => OrderDAO.Instance.GetOrderDetails(orderId);
    }
}
