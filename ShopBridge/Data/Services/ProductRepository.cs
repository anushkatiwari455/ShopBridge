using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopBridge.Data.DTO;
using ShopBridge.Data.Entities;
using ShopBridge.Data.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Data.Services
{
    public class ProductRepository:IProductRepository
    {
        private readonly ShopBridgeStoreContext _shopBridgeStoreContext;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(ShopBridgeStoreContext shopBridgeStoreContext,ILogger<ProductRepository> logger)
        {

            _shopBridgeStoreContext = shopBridgeStoreContext;
            _logger = logger;
        }
        public async Task<List<Product>> GetProductsAsync(string sortBy,string search, int? pageNumber)
        {
            var logMessage = string.Empty;
            try
            {
                var products = await _shopBridgeStoreContext.Products.ToListAsync();
                if(products!=null)
                {
                    switch (sortBy)
                    {
                        case "name_desc":
                            products = await _shopBridgeStoreContext.Products.OrderByDescending(x => x.Name).ToListAsync();
                            break;
                        case "price_desc":
                            products = await _shopBridgeStoreContext.Products.OrderByDescending(x => x.Price).ToListAsync();
                            break;
                        case "description_desc":
                            products = await _shopBridgeStoreContext.Products.OrderByDescending(x => x.Description).ToListAsync();
                            break;
                        case "price_asc":
                            products = await _shopBridgeStoreContext.Products.OrderBy(x => x.Price).ToListAsync();
                            break;
                        case "description_asc":
                            products = await _shopBridgeStoreContext.Products.OrderBy(x => x.Description).ToListAsync();
                            break;
                        default:
                            products = await _shopBridgeStoreContext.Products.OrderBy(x => x.Name).ToListAsync();
                            break;
                    }
                    if (search != null)
                    {
                        products = await _shopBridgeStoreContext.Products.Where(x => x.Name.Contains(search)).ToListAsync();
                    }
                    //pagination
                    int pageSize = 5;
                    products = PaginatedList<Product>.Create(products.AsQueryable(), pageNumber ?? 1, pageSize);
                    logMessage = "Returned products count: " + products.Count();
                    _logger.LogInformation(logMessage);
                    return products;
                }
                else
                {
                    logMessage = "Products Not Found";
                    _logger.LogError(logMessage);
                    return null;
                }               
            }
            catch(DbUpdateConcurrencyException e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return null;
            }
            catch (Exception e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return null;
            }

        }
        public async Task<Product> GetProductsByIdAsync(int id)
        {
            var logMessage = string.Empty;
            try
            {
                var product = await _shopBridgeStoreContext.Products.FirstOrDefaultAsync(x=>x.Id==id);
                if (product != null)
                {
                    logMessage = "Returned product";
                    _logger.LogInformation(logMessage);
                    return product;
                }
                else
                {
                    logMessage = "Products Not Found";
                    _logger.LogError(logMessage);
                    return null;
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return null;
            }
            catch (Exception e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return null;
            }
        }

        public async Task<int> AddProductsAsync(ProductDTO product)
        {
            var logMessage = string.Empty;
            try
            {
                var products = new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                };
                Product productAdded= _shopBridgeStoreContext.Products.Add(products).Entity;
                await _shopBridgeStoreContext.SaveChangesAsync();

                if (productAdded != null)
                {
                    logMessage = "Product added with Id: " + productAdded.Id;
                    _logger.LogInformation(logMessage);
                    return productAdded.Id;
                }
                else
                {
                    logMessage = "Product could not be created";
                    _logger.LogError(logMessage);
                    return 0;
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return 0;
            }
            catch (Exception e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return 0;
            }
        }
        public async Task<bool> UpdateProductAsync(int id,ProductDTO product)
        {
            var logMessage = string.Empty;
            bool isUpdated = false;
            try
            {
                var products = new Product()
                {
                    Id=id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                };
                _shopBridgeStoreContext.Products.Update(products);
                if (await _shopBridgeStoreContext.SaveChangesAsync() != 0)
                {
                    logMessage = "Product updated" ;
                    _logger.LogInformation(logMessage);
                    isUpdated = true;
                }
                else
                {
                    logMessage = "Product Not Updated";
                    _logger.LogError(logMessage);
                }
                return isUpdated;
            }
            catch (DbUpdateConcurrencyException e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return false;
            }
            catch (Exception e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return false;
            }
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var logMessage = string.Empty;
            bool isDeleted = false;
            try
            {
                var product = new Product()
                {
                    Id = id
                };
                _shopBridgeStoreContext.Products.Remove(product);

                if (await _shopBridgeStoreContext.SaveChangesAsync()  != 0)
                {
                    logMessage = "Product deleted";
                    _logger.LogInformation(logMessage);
                    isDeleted = true;
                }
                else
                {
                    logMessage = "Product not deleted";
                    _logger.LogError(logMessage);
                }
                return isDeleted;
            }
            catch (DbUpdateConcurrencyException e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return false;
            }
            catch (Exception e)
            {
                logMessage = "Error: " + e.Message;
                _logger.LogError(logMessage);
                return false;
            }
        }

    }
}
