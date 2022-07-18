using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private SalesManagementContext salesManagementContext = new SalesManagementContext();

        public ProductDAO()
        {
        }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Product> GetList() => salesManagementContext.Products.ToList();

        public Product? GetById(int productId) => salesManagementContext.Products.SingleOrDefault(p => p.ProductId.Equals(productId));

        public void Add(Product product)
        {
            if(GetById(product.ProductId) != null)
            {
                throw new Exception("Product ID existed!");
            }
            salesManagementContext = new SalesManagementContext();
            salesManagementContext.Products.Add(product);
            salesManagementContext.SaveChanges();
        }

        public void Delete(Product product)
        {
            salesManagementContext = new SalesManagementContext();
            salesManagementContext.Products.Remove(product);
            salesManagementContext.SaveChanges();
        }
        public void Update(Product updatedProductInfo)
        {
            salesManagementContext = new SalesManagementContext();
            salesManagementContext.Entry<Product>(updatedProductInfo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            salesManagementContext.SaveChanges(true);
            //salesManagementContext.Products.Update(updatedProductInfo);
        }
        public IEnumerable<Product> SearchByName(string searchKey) 
            => salesManagementContext.Products.Where(p => p.ProductName.ToUpper().Contains(searchKey)).ToList();

        public IEnumerable<Product> SearchByUnitPrice(decimal minVal, decimal maxVal)
        {
            if (minVal > maxVal)
            {
                var tmp = maxVal;
                maxVal = minVal;
                minVal = tmp;
            }
            return salesManagementContext.Products.Where(p => (p.UnitPrice.CompareTo(minVal) >= 0 && p.UnitPrice.CompareTo(maxVal) <= 0));
        }

        public IEnumerable<Product> SearchByUnitInStock(int minVal, int maxVal)
        {
            if (minVal > maxVal)
            {
                var tmp = maxVal;
                maxVal = minVal;
                minVal = tmp;
            }
            return salesManagementContext.Products.Where(p => (p.UnitsInStock.CompareTo(minVal) >= 0 && p.UnitsInStock.CompareTo(maxVal) <= 0));
        }

    }
}