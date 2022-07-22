using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        public IEnumerable<Order> GetAll();
        public IEnumerable<Order> GetAllOfMember(int memberId);
        public IEnumerable<Order> GetAllIgnore();
        public Order Add(Order order);
        public void Delete(Order order);
        public void Update(Order order);
        public Order? GetById(int id);
        public IEnumerable<Order> FilterByDate(DateTime start, DateTime end);
        public IEnumerable<Order> SortDescByDate();
        public IEnumerable<OrderDetail> GetOrderDetails(int orderId);
    }
}
