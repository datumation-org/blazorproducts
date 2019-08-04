using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace datumation_products.Shared.Models {
    public class ProviderTypesByState {
        [Key]
        public int NPI { get; set; }
        public string FacetValueState { get; set; }
        public string FacetValuePt { get; set; }

        public int FacetCount { get; set; }

    }
}