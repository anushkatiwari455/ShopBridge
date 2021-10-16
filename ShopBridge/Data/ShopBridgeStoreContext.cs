using Microsoft.EntityFrameworkCore;
using ShopBridge.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Data
{
    public class ShopBridgeStoreContext : DbContext
    {
        public ShopBridgeStoreContext(DbContextOptions<ShopBridgeStoreContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        //public DbSet<ProductBrand> ProductBrands { get; set; }
        //public DbSet<ProductType> ProductTypes { get; set; }
    }
}
