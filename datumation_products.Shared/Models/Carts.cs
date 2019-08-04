using System;
using System.Collections.Generic;

namespace datumation_products.Shared.Models {
    public partial class Carts {
        public int Id { get; set; }
        public string CartId { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }

        public Items Item { get; set; }
    }
}