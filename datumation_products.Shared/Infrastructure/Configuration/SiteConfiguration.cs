using System;
using datumation_products.Shared.Infrastructure.Caching;
using datumation_products.Shared.Infrastructure.Logging;
using datumation_products.Shared.StartupModels;
using Microsoft.Extensions.Configuration;

namespace datumation_products.Shared.Infrastructure.Configuration {
    public class SiteConfiguration : ISiteConfiguration {

        private IServiceProvider _configuration;
        public SiteConfiguration (IServiceProvider configuration) {
            _configuration = configuration;
        }

        public AppSettings AppSettings {
            get {
                return ServiceProviderServiceExtensions.GetService<AppSettings> (_configuration);
            }
        }

        public ICacheProvider AppCache {
            get {
                return ServiceProviderServiceExtensions.GetService<ICacheProvider> (_configuration);
            }
        }

        public ILogFactory Logger {
            get {
                return ServiceProviderServiceExtensions.GetService<ILogFactory> (_configuration);
            }
        }

    }
}