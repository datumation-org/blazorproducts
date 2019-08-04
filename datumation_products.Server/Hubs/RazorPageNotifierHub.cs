using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datumation_products.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace datumation_products.Server.Hubs {
    public class RazorPageNotifierHub : Hub {
        ShoppingCartService _shop;
        public RazorPageNotifierHub (ShoppingCartService shop) {
            _shop = shop;
        }
        public async Task Reload () => await Clients.All.SendAsync ("Reload");

        public async Task Refresh (string id) {
            await _shop.AddToCart (Convert.ToInt32 (id));
            var c = await _shop.GetCartItems ();
            await Clients.All.SendAsync ("Refresh", new { count = c.Count });
        }
    }
}