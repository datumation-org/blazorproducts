using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datumation_products.Shared.Models {
    public class UserItems {
        public int ID { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ItemPictureUrl { get; set; }
        public string InternalImage { get; set; }
        public int RecordCount { get; set; }
        public string Description { get; set; }
        public string RoutePath { get; set; }
        public string StateName { get; set; }
        public string Specialty { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DownloadLink { get; set; }
        public string FileName { get; set; }
        public string NormalizedUserName { get; set; }
    }
}