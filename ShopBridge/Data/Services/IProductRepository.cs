using ShopBridge.Data.DTO;
using ShopBridge.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Data.Services
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetProductsAsync(string sortBy,string search, int? pageNumber);
        public  Task<Product> GetProductsByIdAsync(int id);
        public  Task<int> AddProductsAsync(ProductDTO product);
        public Task<bool> UpdateProductAsync(int id, ProductDTO product);
        public  Task<bool> DeleteProductAsync(int id);
    }
}
