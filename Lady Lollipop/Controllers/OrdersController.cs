﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lady_Lollipop.Data;
using Lady_Lollipop.Models;
using Lady_Lollipop.Data.Services;
using Lady_Lollipop.Data.Cart;
using Lady_Lollipop.Data.ViewModel;
using System.Security.Claims;

namespace Lady_Lollipop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ISweetsService _sweetService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;

        public OrdersController(ISweetsService sweetService, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            _sweetService = sweetService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var orders =await _ordersService.GetOrderByUserIdAndRoleAsync(userId,userRole);
            return View(orders);
        }


        // GET: Orders
        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(response);
        }

        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            var item = await _sweetService.GetSweetByIdAsync(id);
            if(item != null)
            {
                _shoppingCart.AddItemToCart(item);
                
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<IActionResult> RemoveFromShoppingCart(int id)
        {
            var item = await _sweetService.GetSweetByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.RemoveFromCart(item);

            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            return View("OrderCompleted");

        }
    }
}
