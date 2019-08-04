using datumation_products.Shared.Models;

namespace datumation_products.Shared.ViewModels {
    public class ClientCard {

        public Items Item { get; set; }

        public string Number { get; set; }

        public int ExpYear { get; set; }

        public int ExpMonth { get; set; }

        public string Cvc { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }

        public int Amount { get; set; }

        public string AddressCity { get; set; }

        public string AddressState { get; set; }

        public string AddressZip { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine1 { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }
    }
}