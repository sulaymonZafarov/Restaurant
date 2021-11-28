﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Context;
using Restaurant.Models;
using Restaurant.Models.Restaurant;
using Restaurant.Services.RestInfos;
using Restaurant.Services.RestMenus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
 
namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestaurantContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly RestInfoService _restInfoService;
         private readonly RestMenusService _restMenusService;
        private readonly ShopCart _shopCart;
        //private readonly ControllerContext _controllerContext;
        //private readonly HomeController _homeController;

        private List<ShopCartItem> listOfshopingCartModels;


        public HomeController(ILogger<HomeController> logger,
            RestInfoService restInfoService,
            RestMenusService restMenusService,
            RestaurantContext context,
            ShopCart shopCart
            //HomeController homeController,
            //ControllerContext controllerContext
            
            )
        {
            _context = context;
            _logger = logger;
            _restInfoService = restInfoService;
            _shopCart = shopCart;
            //_homeController = homeController;
            //_controllerContext = controllerContext;

            _restMenusService = restMenusService;
            listOfshopingCartModels = new List<ShopCartItem>();
        }
 
        public int  GetCountItems()
        {
            _shopCart.listShopItems = _shopCart.getShopItems();
            if (_shopCart.listShopItems.Count == 0)
            {
                ViewBag.Message = "";
                var count = _shopCart.listShopItems.Count;
                ViewBag.Count = count;

            }
            else
            {
                var count = _shopCart.listShopItems.Count;
                ViewBag.Count = count;

            }
            return _shopCart.listShopItems.Count;
        }

        public async Task<IActionResult> Index()
        {
            GetCountItems();
            var allRest = await _restInfoService.GetAll();
            // _shopCart.listShopItems = _shopCart.getShopItems();
            //if (_shopCart.listShopItems.Count == 0)
            //{
            //      ViewBag.Message = "";
            //    var count = _shopCart.listShopItems.Count;
            //    ViewBag.Count = count;
            //}
            return View(allRest);
        }

        // GET: RestMenus1
         public async Task<IActionResult> GetMenu(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var restMenuById = await _restMenusService.GetMenuById(id);
            //_shopCart.listShopItems = _shopCart.getShopItems();
            //if (_shopCart.listShopItems.Count == 0)
            //{
            //    ViewBag.Message = "";
            //    var count = _shopCart.listShopItems.Count;
            //    ViewBag.Count = count;
            //}
            GetCountItems();

            return View(restMenuById);

        }

        private IActionResult NotFound()
        {
            throw new NotImplementedException();
        }

       

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(model: new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
