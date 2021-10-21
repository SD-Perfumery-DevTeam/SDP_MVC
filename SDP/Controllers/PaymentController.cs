using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.SDP.SDPCore;
using Microsoft.SDP.SDPInfrastructure.Services;
using SDPCore.Models.AccountModel;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;

        public PaymentController(IConfiguration config)
        {
            this._config = config;
        }

        public ActionResult OrederStatus()
        {
            return View();
        }

        public ActionResult Payment(string stripeToken, string stipeEmail, Key keys)
        {

            try
            {
                StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
                var cutomers = new CustomerService();
                var charges = new ChargeService();
                var customer = cutomers.Create(new CustomerCreateOptions
                {
                    Email = stipeEmail,
                    Source = stripeToken
                });

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = (long)keys.amount,
                    Description = keys.descirption,
                    Currency = keys.currency,
                    Customer = customer.Id.ToString(),
                    ReceiptEmail = customer.Email
                });

                if (charge.Paid)
                {
                    ViewService.getCustomerFromList(HttpContext.Session.GetString("Id")).cart = new Microsoft.SDP.SDPCore.Models.Cart();
                    return View(charge);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction();
        }
    }
}
