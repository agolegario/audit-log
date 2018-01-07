using System;
using System.Collections.Generic;
using System.Linq;
using Company.Poc.WebApi.Mocks;
using Company.Poc.WebApi.Models;

namespace Company.Poc.WebApi.FakeRepository
{
    public interface IProductRepository
    {
        IList<Product> GetAll();
        Product GetById(int id);
        Product Add(Product value);
        Product Update(Product value);
        bool Remove(Product value);
        IEnumerable<Product> Find(Func<Product, bool> predicate);
    }

    public class ProductRepository : IProductRepository
    {
        public IList<Product> GetAll()
        {
            return ProductMock.Products;
        }

        public Product GetById(int id)
        {
            return ProductMock.Products.FirstOrDefault(t => t.Id == id);
        }

        public Product Add(Product value)
        {
            var key = ProductMock.Products.Max(t => t.Id);
            value.Id = key + 1;
            ProductMock.Products.Add(value);
            return value;
        }

        public Product Update(Product value)
        {
            foreach (var produto in ProductMock.Products)
            {
                if (produto.Id != value.Id) continue;
                produto.Description = value.Description;
                produto.Price = value.Price;
            }

            return value;
        }

        public bool Remove(Product value)
        {
            return !ProductMock.Products.Remove(value);
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return ProductMock.Products.Where(predicate).ToList();
        }
    }
}