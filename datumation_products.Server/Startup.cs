using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using datumation_products.Server.Services;
using datumation_products.Shared.Infrastructure.Caching;
using datumation_products.Shared.Infrastructure.Configuration;
using datumation_products.Shared.Infrastructure.Logging;
using datumation_products.Shared.StartupModels;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace datumation_products.Server {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc ();
            services.AddResponseCompression (options => {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat (new [] {
                    MediaTypeNames.Application.Octet,
                        WasmMediaTypeNames.Application.Wasm,
                });
            });
            services.AddSingleton<ILogFactory, Logger> ();
            services.AddSingleton<ICacheProvider, MemoryCacheProvider> ();
            services.AddSingleton<ISiteConfiguration, SiteConfiguration> ();
            services.AddSingleton (LoadAppConfig ());
            services.AddSingleton<IShoppingCartService, ShoppingCartService> ();
            _services = services.BuildServiceProvider ();
            LoadConfig ();

        }
        private IServiceProvider _services { get; set; }
        private void LoadConfig () {

            SiteConfiguration s =
                new SiteConfiguration (_services);
            ConfigurationFactory.Initialize (s);

        }
        private AppSettings LoadAppConfig () {
            var appsett = File.ReadAllText (Environment.CurrentDirectory + "//" + "appsettings.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AppSettings> (appsett);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            app.UseResponseCompression ();

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseMvc (routes => {
                routes.MapRoute (name: "default", template: "{controller}/{action}/{id?}");
            });

            app.UseBlazor<Client.Startup> ();
        }
    }
}