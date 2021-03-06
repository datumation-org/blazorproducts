using System;
using System.Collections.Generic;

namespace datumation_products.Shared.Models {
    public partial class Orders {
        public Orders () {
            OrderDetails = new HashSet<UserOrderDetails> ();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public DateTime Experation { get; set; }
        public bool SaveInfo { get; set; }

        public ICollection<UserOrderDetails> OrderDetails { get; set; }
    }
}