using System;
using System.Collections.Generic;
using System.Text;
using datumation_products.Shared.Models;

namespace datumation_products.Shared.ViewModels {
    public class ShoppingCartViewModel {
        public List<Carts> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}