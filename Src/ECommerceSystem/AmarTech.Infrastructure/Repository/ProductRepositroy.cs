using AmarTech.Domain.Entities;
using AmarTech.Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AmarTech.Infrastructure.Repository
{
    public class ProductRepositroy : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private static readonly char[] _splitByComma = new[] { ',' };

        public ProductRepositroy(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }

        public IEnumerable<Product> SkipAndTake(int pageSize, int pageNumber, string? searchQuery = null, string? includeProperties = null)
        {
            IQueryable<Product> query = _db.Products;

            // Apply search filter if needed
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p =>
                    p.Title.Contains(searchQuery) ||
                    p.Description.Contains(searchQuery));
            }

            // Include related entities if specified
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                             .Split(_splitByComma, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            // Apply pagination
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }
        public void ReduceStockCount(List<ShoppingCart> cartList)
        {
            foreach (var cart in cartList)
            {
                var product = _db.Products.FirstOrDefault(p => p.Id == cart.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= cart.Count;
                }
            }
        }

        public int GetAllProductsCount(string? searchQuery = null)
        {
            IQueryable<Product> query = _db.Products;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                query = query.Where(p => (p.Title != null && p.Title.ToLower().Contains(searchQuery)) ||
                                         (p.Description != null && p.Description.ToLower().Contains(searchQuery)));
            }

            return query.Count();
        }
    }
}
