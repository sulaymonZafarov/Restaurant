﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Context;
using Restaurant.Models.Interfaces;
using Restaurant.Models.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAllOrders  _allOrders;
        private readonly ShopCart _shopCart;
        private readonly RestaurantContext _context;

        public OrderController(IAllOrders allOrders,
            ShopCart shopCart,
            RestaurantContext Context
            )
        {
            this._allOrders = allOrders;
            this._shopCart = shopCart;
            this._context = Context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
           
            return View();
        }
        [HttpGet]
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
             var restaurantContext = _context.OrderDetails.Include(o => o.Orders).Where(c=> c.Orders.Status == "Обрабатывается");
            
            return View(await restaurantContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            _shopCart.listShopItems = _shopCart.getShopItems();
            if (_shopCart.listShopItems.Count == 0)
            {
                ModelState.AddModelError("","У Вас должны быть товары!");
                 ViewBag.Message = "Корзина пуста, У Вас должны быть товары!";
 
            }
            if (ModelState.IsValid)
            {
                _allOrders.createOrder(order);
                return RedirectToAction("Complete");
             }

            return View(order);
        }
        public IActionResult Complete()
        {
            ViewBag.Message = "Заказ успешно обработан";
            return View();
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var orderDetail = await _context.Orders
            //   .Include(o => o.OrderDetails)
            //   .FirstOrDefaultAsync(m => m.Id == id);
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            //var orderDetail = await _context.OrderDetails.FindAsync(id);
            //_context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost, ActionName("Ready")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderReady(Guid id)
        {
            var orders = await _context.Orders.FindAsync(id);

            orders.Status = "Готово";
           // _context.Orders.Remove(orders);
            //var orderDetail = await _context.OrderDetails.FindAsync(id);
            //_context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


         
    }
}
