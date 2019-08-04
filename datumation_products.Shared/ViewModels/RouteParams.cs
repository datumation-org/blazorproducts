namespace datumation_products.Shared.ViewModels {

    public enum RouteTypeEnm {
        SpecialtyIndex = 1,
        StateIndex = 2,
        ComboIndex = 3
    }
    public class RouteParam {
        public int CategoryId { get; set; }
        public RouteTypeEnm RouteType { get; set; }
        public string RouteParamValue { get; set; }
        public string Specialty { get; set; }
        public string StateName { get; set; }
    }
}