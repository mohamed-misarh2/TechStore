﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechStore.Application.Services;


namespace TechStore.ViewUser.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View("checkout");
        }

        public async Task<IActionResult> GetAllOrders(string UserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId = userId;
            var Orders = await _orderService.GetOrdersByUserIdAsync(UserId);
            return View("Orders", Orders);
        }

        public async Task<IActionResult> GetAllOrderItems(int OrderId)
        {
            var OrderItems = await _orderService.GetOrderDetails(OrderId);
            return View("OrderItems", OrderItems);
        }


    }
}