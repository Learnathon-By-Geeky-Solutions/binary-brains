using AmarTech.Domain.Entities;
using AmarTech.Infrastructure.Repository;
using AmarTech.Infrastructure.Repository.IRepository;
using AmarTech.Application.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace AmarTech.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

        }

        public void AddShoppingCart(ShoppingCart shoppingCart)
        {
            _shoppingCartRepository.Add(shoppingCart);
            _unitOfWork.Commit();
        }

        public void DeleteShoppingCart(int? id)
        {
            if (id != null)
            {
                var shoppingcart = GetShoppingCartById(id);
                if (shoppingcart != null)
                {
                    _shoppingCartRepository.Remove(shoppingcart);
                    _unitOfWork.Commit();
                }

            }


        }

        public ShoppingCart? GetShoppingCartById(int? id, bool track = false)
        {
            return _shoppingCartRepository.Get(u => u.Id == id, tracked: track);
        }

        public ShoppingCart? GetShoppingCartByUserAndProduct(string userId, int productId)
        {
            return _shoppingCartRepository.Get(u => u.ApplicationUserId == userId && u.ProductId == productId);
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            var existingCart = _shoppingCartRepository.Get(u => u.Id == shoppingCart.Id);
            if (existingCart != null)
            {
                existingCart.Count = shoppingCart.Count;
                _shoppingCartRepository.Update(existingCart);
                _unitOfWork.Commit();
            }
        }

        public IEnumerable<ShoppingCart> GetShoppingCartsByUserId(string userId)
        {
            return _shoppingCartRepository.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Product" // Ensure Product is loaded
            ) ?? new List<ShoppingCart>();
        }

        public void RemoveRange(List<ShoppingCart> shoppingCarts)
        {
            _shoppingCartRepository.RemoveRange(shoppingCarts);
            _unitOfWork.Commit();
        }

        public ShoppingCart CreateCartWithProduct(Product product)
        {
            return new ShoppingCart
            {
                Product = product,
                ProductId = product.Id,
                Count = 1
            };
        }

        public bool AddOrUpdateShoppingCart(ShoppingCart shoppingCart, string userId)
        {
            if (string.IsNullOrEmpty(userId) || shoppingCart == null || shoppingCart.ProductId == 0)
            {
                return false;
            }

            shoppingCart.ApplicationUserId = userId;

            var cartFromDb = GetShoppingCartByUserAndProduct(userId, shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                UpdateShoppingCart(cartFromDb);
            }
            else
            {
                AddShoppingCart(shoppingCart);
            }

            _unitOfWork.Commit();
            return true;
        }

        public ShoppingCartVM GetShoppingCartVM(string? userId)
        {


            var shoppingCartList = GetShoppingCartsByUserId(userId ?? "") ?? new List<ShoppingCart>(); // Ensure not null


            var shoppingCartVM = new ShoppingCartVM
            {
                ShoppingCartList = shoppingCartList,
                OrderHeader = new OrderHeader
                {
                    OrderTotal = (double)shoppingCartList.Where(cart => cart.Product != null) // Avoid null references
                                             .Sum(cart => cart.Product.Price * cart.Count)
                }
                


            };
            return shoppingCartVM;
        }

        public List<ShoppingCart> RemoveShoppingCarts(OrderHeader? orderHeader)
        {
            if (orderHeader != null)
            {
                var shoppingCarts = GetShoppingCartsByUserId(orderHeader.ApplicationUserId).ToList();
                RemoveRange(shoppingCarts);
                _unitOfWork.Commit();
                return shoppingCarts;
            }
            return new List<ShoppingCart>();
            
        }
        public void Plus(int cartId)
        {
            var cartFromDb = GetShoppingCartById(cartId);
            if (cartFromDb == null)
            {
                return;
            }

            cartFromDb.Count += 1;
            UpdateShoppingCart(cartFromDb);
            _unitOfWork.Commit();

        }

        public void Minus(int cartId)
        {
            var cartFromDb = GetShoppingCartById(cartId);
            if (cartFromDb == null)
            {
                return;
            }

            if (cartFromDb.Count <= 1)
            {
                DeleteShoppingCart(cartId);
                _httpContextAccessor.HttpContext?.Session.SetInt32(SD.SessionCart,
GetShoppingCartByUserId(cartFromDb.ApplicationUserId).Count());

            }
            else
            {
                cartFromDb.Count -= 1;
                UpdateShoppingCart(cartFromDb);
            }

            _unitOfWork.Commit();
        }

        public void RemoveCartValue(int cartId)
        {
            var cartFromDb = GetShoppingCartById(cartId);
            if (cartFromDb == null)
            {
                return;
            }
            DeleteShoppingCart(cartId);
            _httpContextAccessor.HttpContext?.Session.SetInt32(SD.SessionCart,
            GetShoppingCartByUserId(cartFromDb.ApplicationUserId).Count());
            _unitOfWork.Commit();
        }

        public IEnumerable<ShoppingCart> GetShoppingCartByUserId(string userId)
        {
            return _shoppingCartRepository.GetAll(u => u.ApplicationUserId == userId);
        }

        public ShoppingCartVM GetShoppingCartVMForSummaryPost(IEnumerable<ShoppingCart> shoppingCartList,ApplicationUser applicationUser,string userId)
        {
            double orderTotal = shoppingCartList
             .Where(cart => cart.Product != null)
             .Sum(cart => (double)cart.Product.Price * cart.Count);


            var shoppingCartVM= new ShoppingCartVM
            {
                ShoppingCartList = shoppingCartList,
                OrderHeader = new OrderHeader
                {
                    ApplicationUserId = userId,
                    OrderDate = DateTime.Now,
                    OrderTotal = orderTotal,
                    Name = applicationUser.Name,
                    PhoneNumber = applicationUser.PhoneNumber ?? string.Empty,
                    StreetAddress = applicationUser.StreetAddress ?? string.Empty,
                    City = applicationUser.City ?? string.Empty,
                    State = applicationUser.State ?? string.Empty,
                    PostalCode = applicationUser.PostalCode ?? string.Empty
                }
            };
            foreach (var item in shoppingCartVM.ShoppingCartList)
            {
                if (item.Product != null)
                {
                    item.Price = (double)item.Product.Price;
                }
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            return shoppingCartVM;
        }

        public SessionCreateOptions CheckOutForUser(ShoppingCartVM shoppingCartVM)
        {
            var domain = "https://localhost:7000/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + "customer/cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var item in shoppingCartVM.ShoppingCartList)
            {
                if (item.Product == null) continue;

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(0 * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }
            return options;
        }
    }
}
