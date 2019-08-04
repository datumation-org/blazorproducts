using System;
using System.IO;
using datumation_products.Shared.StartupModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datumation_products.Server {

    public class DatumationProgramStartup {
        public AppSettings AppConfig { get; set; }
        public IConfiguration Configuration { get; set; }
        public IServiceCollection Services { get; set; }
        public IServiceProvider Provider { get; set; }

        public DatumationProgramStartup () {
            AppConfig = new AppSettings ();
            Services = new ServiceCollection ();

        }

        public void ConfigureAppConfiguration (string configPath) {
            // build configuration
            Configuration = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile (configPath, false)
                .Build ();

            Services.AddOptions ();
            Services.AddSingleton (Configuration);

        }
        public void MakeServiceProvider () {
            Provider = Services.BuildServiceProvider ();
        }

        public void ConfigureServices<T, U> ()
        where T : class
        where U : class, T {
            Services.AddSingleton<T, U> ();
        }

    }
}