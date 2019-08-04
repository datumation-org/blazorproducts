using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datumation_products.Server.Models;
using datumation_products.Server.Services;
using datumation_products.Shared;
using Microsoft.AspNetCore.Mvc;

namespace datumation_products.Server.Controllers {
    [Route ("api/[controller]")]
    public class ShoppingController : Controller {

        private readonly IShoppingCartService _shop;
        public ShoppingController (Services.IShoppingCartService shop) {
            _shop = shop;
        }

        [HttpGet]
        [Route ("ItemList")]
        public async Task<IEnumerable<Items>> ItemList () {
            return await _shop.getItems ();
        }
    }
}