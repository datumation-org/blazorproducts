using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datumation_products.Shared.Models {
    public class UserOrders {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemPurchaseId { get; set; }
    }
}