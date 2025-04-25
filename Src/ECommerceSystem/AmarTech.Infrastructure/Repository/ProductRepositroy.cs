using AmarTech.Domain.Entities;
using AmarTech.Infrastructure.Repository.IRepository;
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
        public ProductRepositroy(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }

        public IEnumerable<Product> SkipAndTake(int productsPerPage,int pageNumber)
        {
            IQueryable<Product> query = _db.Products;

            query = query.Skip((pageNumber-1) * productsPerPage).Take(productsPerPage);

            return query.ToList();
         
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


    }
}
