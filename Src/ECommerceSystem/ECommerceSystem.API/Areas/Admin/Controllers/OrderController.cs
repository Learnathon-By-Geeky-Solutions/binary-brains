﻿using ECommerceSystem.Models;
using ECommerceSystem.Service.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]


    public class OrderController : Controller
    {
        private readonly IOrderHeaderService _orderHeaderService;
        private readonly IOrderDetailService _orderDetailService;

        public OrderController(IOrderHeaderService orderHeaderService,IOrderDetailService orderDetailService)
        {
            _orderHeaderService = orderHeaderService;
            _orderDetailService = orderDetailService;
        }




        public IActionResult Index(string? status) 
        {
            IEnumerable<OrderHeader> orderData;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
               orderData = _orderHeaderService.GetAllOrderHeaders("ApplicationUser");
            }

            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;

                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                orderData=_orderHeaderService.GetAllOrderHeadersById(userId,"ApplicationUser");
            }

            if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
            {
                orderData = orderData.Where(u => u.PaymentStatus.ToLower() == status.ToLower());
            }


            

            return View(orderData);
        }
        public IActionResult Details(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                orderHeader = _orderHeaderService.GetOrderHeaderById(id, "ApplicationUser"),
                orderDetails=_orderDetailService.GetAllOrders(id,"Product")
            };

            return View(orderVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult Details(OrderVM orderVM)
        {
            var orderHeaderFromDb = _orderHeaderService.GetOrderHeaderById(orderVM.orderHeader.Id);

            orderHeaderFromDb.Name = orderVM.orderHeader.Name;
            orderHeaderFromDb.PhoneNumber = orderVM.orderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = orderVM.orderHeader.StreetAddress;
            orderHeaderFromDb.City = orderVM.orderHeader.City;
            orderHeaderFromDb.State = orderVM.orderHeader.State;
            orderHeaderFromDb.PostalCode = orderVM.orderHeader.PostalCode;
            if (!string.IsNullOrEmpty(orderVM.orderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = orderVM.orderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(orderVM.orderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = orderVM.orderHeader.TrackingNumber;
            }
            _orderHeaderService.UpdateOrderHeader(orderHeaderFromDb);
    


            return RedirectToAction(nameof(Details), new { id = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing(OrderVM orderVM)
        {
            _orderHeaderService.UpdateStatus(orderVM.orderHeader.Id, SD.StatusInProcess);

            return RedirectToAction(nameof(Details), new { id=orderVM.orderHeader.Id });
        }

    }
}
