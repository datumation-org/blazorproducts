using System;
using System.Collections.Generic;

namespace datumation_products.Shared.Models {
    public partial class Categories {
        public Categories () {
            Items = new HashSet<Items> ();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Items> Items { get; set; }
    }
}