using System;
using System.Collections.Generic;

namespace datumation_products.Shared.Models {
    public partial class AspNetUserLogins {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }

        public AspNetUsers User { get; set; }
    }
}