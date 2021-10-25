using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SDP.SDPCore;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using Microsoft.SDP.SDPInfrastructure.Services;
using SDPCore.Dtos;
using SDPCore.Models.AccountModel;
using SDPWeb.ViewModels;
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
        private ProductDbContext _db;
        private readonly IDbContextFactory<ProductDbContext> _contextFactory;
        private IDbRepo _dbRepo;

        public PaymentController(IConfiguration config, ProductDbContext db, IDbContextFactory<ProductDbContext> contextFactory, IDbRepo dbRepo)
        {
            _config = config;
            _db = db;
            _contextFactory = contextFactory;
            _dbRepo = dbRepo;
        }

        [HttpPost]
        public async Task<ActionResult> PaymentAsync(CheckoutView checkoutView,string stripeToken)
        {
            try
            {
                StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
                var cutomers = new CustomerService();
                var charges = new ChargeService();
                var customer = cutomers.Create(new CustomerCreateOptions
                {

                    Email = checkoutView.delivery.email,
                    Source = stripeToken
                });

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = (long)checkoutView.amount,
                    Description = checkoutView.key.descirption,
                    Currency = checkoutView.key.currency,
                    Customer = customer.Id.ToString(),
                    ReceiptEmail = customer.Email
                });

                if (charge.Paid)
                {
                    OrderDataTransfer orderDataTransfer = null;
                    var currentIdentityCutomer = await _dbRepo.getCustomerAsync(HttpContext.Session.GetString("Id"));
                    var currentCutomer = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"));
                    checkoutView.delivery.deliveryDate = DateTime.Now;
                    using (var context = _contextFactory.CreateDbContext())
                    {
                        orderDataTransfer = currentCutomer.cart.turnCartToOrder(context.order.Count() + 1, currentIdentityCutomer, checkoutView.delivery, checkoutView.amount, "payed", DateTime.Now, Consts.OrderStatus.pendingAction, context.product.ToList());
                    }
                    _db.order.Add(orderDataTransfer.order);
                    await _db.SaveChangesAsync();
                    currentCutomer.cart = new Cart();
                    return View(orderDataTransfer.order.orderNo);
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
