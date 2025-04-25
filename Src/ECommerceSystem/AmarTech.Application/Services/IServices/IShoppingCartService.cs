using AmarTech.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace AmarTech.Application.Services.IServices
{
    public interface IShoppingCartService
    {

        ShoppingCart? GetShoppingCartById(int? id, bool track = false);
        void AddShoppingCart(ShoppingCart shoppingCart);
        void UpdateShoppingCart(ShoppingCart shoppingCart);
        ShoppingCart? GetShoppingCartByUserAndProduct(string userId, int productId);
        IEnumerable<ShoppingCart> GetShoppingCartsByUserId(string userId);
        void RemoveRange(List<ShoppingCart> shoppingCarts);

        void DeleteShoppingCart(int? id);

        ShoppingCart CreateCartWithProduct(Product product);

        bool AddOrUpdateShoppingCart(ShoppingCart shoppingCart, string userId);

        ShoppingCartVM GetShoppingCartVM(string? userId);

        List<ShoppingCart> RemoveShoppingCarts(OrderHeader? orderHeader);

        void Plus(int cartId);

        void Minus(int cartId);

        void RemoveCartValue(int cartId);
        IEnumerable<ShoppingCart> GetShoppingCartByUserId(string userId);

        ShoppingCartVM GetShoppingCartVMForSummaryPost(IEnumerable<ShoppingCart> shoppingCartList, ApplicationUser applicationUser, string userId);

        SessionCreateOptions CheckOutForUser(ShoppingCartVM shoppingCartVM);



    }
}
