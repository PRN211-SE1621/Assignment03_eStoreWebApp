using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        public IEnumerable<Product> GetAll();
        public Product? GetById(int id);
        public void CreateProduct(Product p);

        public void DeleteProduct(int id);

        public void UpdateProduct(Product updatedProductInfo);

        public Product GetProductById(int id);

        public IEnumerable<Product> GetAllProducts();

        public List<Product> SearchProductByName(string name);

        public List<Product> SearchProductById(string id);

        public List<Product> SearchProductByIdAndName(string searchName, string searchId);

        public IEnumerable<Product> SearchProductByIdAndNameAndUnitPriceAndUnitInStock(string searchId, string searchName, string searchUnitPriceMin, string searchUnitPriceMax , string searchUnitInStockMin, string searchUnitInStockMax);


    }
}
