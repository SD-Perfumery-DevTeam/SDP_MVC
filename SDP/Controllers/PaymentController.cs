using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SDP.SDPCore;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPInfrastructure.Services;
using Newtonsoft.Json;
using SDPCore.Dtos;
using SDPWeb.ViewModels;
using Stripe;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext _db;
        private readonly IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> _contextFactory;
        private UserManager<IdentityUser> _userManager { get; }
        private IDbRepo _dbRepo;

        public PaymentController(IConfiguration config, Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext db, IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> contextFactory, UserManager<IdentityUser> userManager, IDbRepo dbRepo)
        {
            _config = config;
            _db = db;
            _contextFactory = contextFactory;
            _userManager = userManager;
            _dbRepo = dbRepo;
        }

  
       


         [HttpGet]
        public async Task<ActionResult> PaymentAsync(string checkoutViewJson, string stripeToken)
        {
                try
                {
                    CheckoutView checkoutView = JsonConvert.DeserializeObject<CheckoutView>(checkoutViewJson);
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
                            orderDataTransfer = currentCutomer.cart.turnCartToOrder(context.order.Count() + 1, currentIdentityCutomer, checkoutView.delivery, checkoutView.amount / 100, "paied", DateTime.Now, Consts.OrderStatus.pendingAction, context.product.Include(m => m.brand).Include(m => m.category).ToList());
                        }

                        foreach (var order in orderDataTransfer.order.orderLine)
                        {
                            order.product = await _db.product.FindAsync(order.product.productId);
                        }
                        _db.order.Add(orderDataTransfer.order);
                        await _db.SaveChangesAsync();


                        currentCutomer.cart = new Cart();
                        return View(orderDataTransfer.order.orderNo);
                    }
           
            }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Payment");
                }
           
            return RedirectToAction("Error", "Payment");
        }
    }
}
