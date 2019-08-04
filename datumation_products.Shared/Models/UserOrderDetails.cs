using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datumation_products.Shared.Models {
    public class UserOrderDetails {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ItemId { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentReceived { get; set; }
        public DateTime EndDate { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string Zip { get; set; }
        public decimal AmountBilled { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}