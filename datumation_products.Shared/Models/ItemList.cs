using System.ComponentModel.DataAnnotations.Schema;

namespace datumation_products.Shared.Models {

    [Table ("ItemProducts")]
    public class ItemList {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string csvFile { get; set; }
        public decimal Price { get; set; }
        public string ItemPictureUrl { get; set; }
        public byte[] InternalImage { get; set; }
        public string StateName { get; set; }
        public string Specialty { get; set; }
        public int RecordCount { get; set; }
        public string RoutePath { get; set; }
        public string Description { get; set; }
    }
}