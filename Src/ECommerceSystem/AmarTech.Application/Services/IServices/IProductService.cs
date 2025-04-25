using AmarTech.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AmarTech.Application.Services.IServices
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(int? id);
        Product? GetProductByIdwithCategory(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int? id);

        IEnumerable<SelectListItem> CategoryList();

        void EditPathOfProduct(Product product, IFormFile? file,string wwwRootPath);
        void CreatePathOfProduct(Product product, IFormFile? file, string wwwRootPath);

        IEnumerable<Product> SkipAndTake(int ? page);
        int CalculateTotalPage();
        void ReduceStockCount(List<ShoppingCart> cartList);
    }
}
