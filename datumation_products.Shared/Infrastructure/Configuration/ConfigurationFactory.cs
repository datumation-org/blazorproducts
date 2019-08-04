using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace datumation_products.Shared.Infrastructure.Configuration
{
    public class ConfigurationFactory
    {
        private static ISiteConfiguration _config;

        public ConfigurationFactory() { }
        public ISiteConfiguration Configuration()
        {
            return _config;
        }

        public static ConfigurationFactory Instance
        {
            get
            {
                return SingletonCreator.CreatorInstance;
            }
        }

        private sealed class SingletonCreator
        {

            private static readonly ConfigurationFactory _instance = new ConfigurationFactory();
            public static ConfigurationFactory CreatorInstance
            {
                get { return _instance; }
            }
        }

        public static void Initialize(ISiteConfiguration c)
        {
            _config = c;
        }
    }
}