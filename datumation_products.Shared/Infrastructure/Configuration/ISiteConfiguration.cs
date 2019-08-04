using datumation_products.Shared.Infrastructure.Caching;
using datumation_products.Shared.Infrastructure.Logging;
using datumation_products.Shared.StartupModels;

namespace datumation_products.Shared.Infrastructure.Configuration {
    public interface ISiteConfiguration {
        AppSettings AppSettings { get; }
        ICacheProvider AppCache { get; }
        ILogFactory Logger { get; }
    }
}