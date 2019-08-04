using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datumation_products.Shared.ViewModels;
using Microsoft.Extensions.Options;
using Stripe;

namespace datumation_products.Server.Services {
    public class PaymentService {
        //{
        //    private readonly PaymentSettings paymentSettings;
        //    private readonly StripeChargeService stripeChargeService;

        //    public PaymentService(IOptions<PaymentSettings> paymentSettings, StripeChargeService stripeChargeService)
        //    {
        //        this.paymentSettings = paymentSettings.Value;
        //        this.stripeChargeService = stripeChargeService;
        //    }

        //    public bool ProcessPayment(string token)
        //    {
        //        var myCharge = new StripeChargeCreateOptions();
        //        // Always set these properties
        //        myCharge.Amount = 50;
        //        myCharge.Currency = "usd";
        //        myCharge.Description = "Premium membership";
        //        myCharge.SourceTokenOrExistingSourceId = token;
        //        // (not required) set this to false if you don't want to capture the charge yet - requires you call capture later
        //        myCharge.Capture = true;
        //        stripeChargeService.ApiKey = paymentSettings.StripePrivateKey;
        //        StripeCharge stripeCharge = stripeChargeService.Create(myCharge);

        //        if (String.IsNullOrEmpty(stripeCharge.FailureCode) && String.IsNullOrEmpty(stripeCharge.FailureMessage))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            //TODO: Handle errors
        //            return false;
        //        }
        //    }
    }
}