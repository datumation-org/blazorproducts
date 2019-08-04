using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using datumation_products.Server.Extensions;
using datumation_products.Shared.Infrastructure.Caching;
using datumation_products.Shared.Infrastructure.Configuration;
using datumation_products.Shared.Models;
using datumation_products.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace datumation_products.Server.Services {
    public interface IDatumCardService {
        CreditCardOptions LoadCardOptions (ClientCard card);
        ClientCard LoadFormToCard (Microsoft.AspNetCore.Http.IFormCollection requestForm, ApplicationUser user);
        void SetStripeApiKey ();
    }

    public class DatumCardCacheService : IDatumCardService {
        private ShoppingCartService _shopService;
        private ICacheProvider _cacheProvider;
        public DatumCardCacheService ([FromServices] ShoppingCartService shopService, [FromServices] ICacheProvider cacheProvider) {
            _shopService = shopService;
            _cacheProvider = cacheProvider;
        }

        public void SetStripeApiKey () {
            StripeConfiguration
                .SetApiKey (ConfigurationFactory.Instance.Configuration ().AppSettings.AppConfiguration.Stripe.PublishableKey);
        }

        private ClientCard LoadCard (Microsoft.AspNetCore.Http.IFormCollection requestForm, ApplicationUser user) {
            var card = new ClientCard ();

            var cvc = ConversionHelpers.ConvertForm (requestForm["Cvc"]);
            var expMonth = ConversionHelpers.ConvertForm (requestForm["ExpMonth"]);
            var expYear = ConversionHelpers.ConvertForm (requestForm["ExpYear"]);
            var cardNumber = ConversionHelpers.ConvertForm (requestForm["Number"]);
            var email = user.Email;
            card.Cvc = cvc;
            card.Email = email;
            card.ExpMonth = Convert.ToInt32 (expMonth);
            card.ExpYear = Convert.ToInt32 (expYear);
            card.Number = cardNumber;
            card.AddressCity = ConversionHelpers.ConvertForm (requestForm["AddressCity"]);
            card.AddressLine1 = ConversionHelpers.ConvertForm (requestForm["AddressLine1"]);
            card.AddressLine2 = ConversionHelpers.ConvertForm (requestForm["AddressLine2"]);
            card.AddressState = ConversionHelpers.ConvertForm (requestForm["AddressState"]);
            card.Name = ConversionHelpers.ConvertForm (requestForm["Name"]);
            // card.Country = ConversionHelpers.ConvertForm(requestForm["Country"]);
            card.AddressZip = ConversionHelpers.ConvertForm (requestForm["AddressZip"]);
            return card;
        }
        private CreditCardOptions CardOptions (ClientCard card) {
            CreditCardOptions cardData = new CreditCardOptions ();
            try {
                cardData = new CreditCardOptions () {
                    Cvc = card.Cvc,
                    Currency = "usd",
                    ExpMonth = ConversionHelpers.ConvertToLong (card.ExpMonth),
                    ExpYear = ConversionHelpers.ConvertToLong (card.ExpYear),
                    AddressCity = card.AddressCity,
                    AddressLine1 = card.AddressLine1,
                    AddressLine2 = card.AddressLine2,
                    AddressState = card.AddressState,
                    Name = card.Number,
                    AddressCountry = card.Country,
                    Number = card.Number,
                    AddressZip = card.AddressZip
                };
            } catch (System.Exception e) {

            }
            return cardData;

        }
        public CreditCardOptions LoadCardOptions (ClientCard card) {

            StringBuilder cacheKey = new StringBuilder ();
            cacheKey.Append ($"CARD_OPTIONS_USER_{card.Name}");

            CreditCardOptions options = new CreditCardOptions ();
            try {
                options = _cacheProvider.Retrieve<CreditCardOptions> (cacheKey.ToString ());
            } catch (System.Exception e) {

                Console.WriteLine ($"ERROR GET FILE DATA: {e.Message}");
            }

            if (options == null) {
                options = CardOptions (card);

                if (options != null) {
                    _cacheProvider.Store (cacheKey.ToString (), options);
                }
            }
            return options;

        }
        public ClientCard LoadFormToCard (Microsoft.AspNetCore.Http.IFormCollection requestForm, ApplicationUser user) {

            StringBuilder cacheKey = new StringBuilder ();
            cacheKey.Append ($"LOAD_FORM_TO_CARD_{user.Email}");

            ClientCard options = new ClientCard ();
            try {
                options = _cacheProvider.Retrieve<ClientCard> (cacheKey.ToString ());
            } catch (System.Exception e) {

                Console.WriteLine ($"ERROR GET FILE DATA: {e.Message}");
            }

            if (options == null) {
                options = LoadCard (requestForm, user);

                if (options != null) {
                    _cacheProvider.Store (cacheKey.ToString (), options);
                }
            }
            return options;

        }
    }

    public class DatumCardService : IDatumCardService {
        private ShoppingCartService _shopService;
        public DatumCardService ([FromServices] ShoppingCartService shopService) {
            _shopService = shopService;
        }
        private ClientCard LoadCard (Microsoft.AspNetCore.Http.IFormCollection requestForm, ApplicationUser user) {
            var card = new ClientCard ();

            var cvc = ConversionHelpers.ConvertForm (requestForm["Cvc"]);
            var expMonth = ConversionHelpers.ConvertForm (requestForm["ExpMonth"]);
            var expYear = ConversionHelpers.ConvertForm (requestForm["ExpYear"]);
            var cardNumber = ConversionHelpers.ConvertForm (requestForm["Number"]);
            var email = user.Email;
            card.Cvc = cvc;
            card.Email = email;
            card.ExpMonth = Convert.ToInt32 (expMonth);
            card.ExpYear = Convert.ToInt32 (expYear);
            card.Number = cardNumber;
            card.AddressCity = ConversionHelpers.ConvertForm (requestForm["AddressCity"]);
            card.AddressLine1 = ConversionHelpers.ConvertForm (requestForm["AddressLine1"]);
            card.AddressLine2 = ConversionHelpers.ConvertForm (requestForm["AddressLine2"]);
            card.AddressState = ConversionHelpers.ConvertForm (requestForm["AddressState"]);
            card.Name = ConversionHelpers.ConvertForm (requestForm["Name"]);
            card.Country = ConversionHelpers.ConvertForm (requestForm["Country"]);
            card.AddressZip = ConversionHelpers.ConvertForm (requestForm["AddressZip"]);
            return card;
        }
        private CreditCardOptions CardOptions (ClientCard card) {
            CreditCardOptions cardData = new CreditCardOptions ();
            try {
                cardData = new CreditCardOptions () {
                    Cvc = card.Cvc,
                    Currency = "usd",
                    ExpMonth = ConversionHelpers.ConvertToLong (card.ExpMonth),
                    ExpYear = ConversionHelpers.ConvertToLong (card.ExpYear),
                    AddressCity = card.AddressCity,
                    AddressLine1 = card.AddressLine1,
                    AddressLine2 = card.AddressLine2,
                    AddressState = card.AddressState,
                    Name = card.Number,
                    AddressCountry = card.Country,
                    Number = card.Number,
                    AddressZip = card.AddressZip
                };
            } catch (System.Exception e) {

            }
            return cardData;

        }
        private void StripeApiKeyLoader () {
            StripeConfiguration
                .SetApiKey (ConfigurationFactory.Instance.Configuration ().AppSettings.AppConfiguration.Stripe.PublishableKey);
        }
        public CreditCardOptions LoadCardOptions (ClientCard card) {
            return CardOptions (card);
        }

        public ClientCard LoadFormToCard (IFormCollection requestForm, ApplicationUser user) {
            return LoadCard (requestForm, user);
        }
        public void SetStripeApiKey () {
            StripeApiKeyLoader ();
        }
    }
}