using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Company.Poc.WebApi.Models;

namespace Company.Poc.WebApi.Mocks
{
    public static class ProductMock
    {
        public static List<Product> Products { get; set; }

        public static void BindProducts()
        {
            Products = new List<Product>
            {
                new Product { Id = 1, Description = "Iphone 6 4.7",Price = 2000m},
                new Product { Id = 2, Description = "Galaxy S8 64gb", Price = 3500m},
                new Product { Id = 3, Description = "Macbook pro 13" ,Price = 7500m},
                new Product { Id = 4, Description = "Macbook pro 15", Price = 9500m},
                new Product { Id = 5, Description = "Moto G5s Plus", Price = 1100m}
            };
        }
    }
}