using System;
using System.Collections.Generic;

namespace datumation_products.Shared.Models {
    public partial class AspNetUserClaims {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public AspNetUsers User { get; set; }
    }
}