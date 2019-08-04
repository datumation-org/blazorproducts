using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace datumation_products.Shared.Models {

    public partial class Items {
        public Items () {
            Carts = new HashSet<Carts> ();
            OrderDetails = new HashSet<UserOrderDetails> ();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string csvFile { get; set; }
        public decimal Price { get; set; }
        public string ColorScheme { get; set; }
        public string ItemPictureUrl { get; set; }
        public string InternalImage { get; set; }
        public string StateName { get; set; }
        public string StateAbbr { get; set; }
        public string Specialty { get; set; }
        public int RecordCount { get; set; }
        public string RoutePath { get; set; }
        public string Description { get; set; }
        public Categories Category { get; set; }
        public ICollection<Carts> Carts { get; set; }
        public ICollection<UserOrderDetails> OrderDetails { get; set; }
    }
}