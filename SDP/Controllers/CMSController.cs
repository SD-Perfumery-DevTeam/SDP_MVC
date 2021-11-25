using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SDPWeb.ViewModels;
using Wkhtmltopdf.NetCore;
using Microsoft.Extensions.Logging;

namespace SDP.Controllers
{
    public class CMSController : Controller
    {
        private UserManager<IdentityUser> _userManager { get; }
        private IEmailSender _emailSender { get; }
        private IDbRepo _dbRepo;
        private readonly IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> _contextFactory;
        // Declare the ILogger object.
        private readonly ILogger<CMSController> _logger;
        // Declare the IGeneratePdf object.
        private readonly IGeneratePdf _generatePdf;

        // ILogger and IGeneratePdf objects inserted into the constructor.
        public CMSController(UserManager<IdentityUser> userManager,
            IEmailSender emailSender,
            IDbRepo dbRepo,
            IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> contextFactory,
            ILogger<CMSController> logger, IGeneratePdf generatePdf)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _dbRepo = dbRepo;
            _contextFactory = contextFactory;
            _logger = logger;
            _generatePdf = generatePdf;
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Index(string Msg)
        {
            ViewData["Msg"] = Msg;
            return View();
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult SendEmailToCustomers(string promoId)
        {
            try
            {
                List<string> emailList = new List<string>();
                foreach (var customer in _dbRepo.GetCustomerList())
                {
                    emailList.Add(customer.Email);
                }
                Promotion promo = _dbRepo.getPromotionByID(promoId);
                _emailSender.sendPromtionEmailAsyncAsync(emailList, promo.title, promo.promoCode, promo.startDate.ToString(), promo.endDate.ToString(), promo.discount.ToString() + "%", promo.product.title);
            }
            catch (Exception ex)
            {
                // If an exception is thrown, log the event.
                _logger.LogError(ex, "Problem in SendEmailToCustomers action method " +
                    "in CMS controller.");
                // Redirect to error page.
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "CMS", new { Msg = "Promotions has been sent" });
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> ViewAllOrdersAsync()
        {
            List<Order> list;
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    list = await context.order.Include(m => m.delivery).ToListAsync();
                    list = list.OrderByDescending(m => m.paymentDate).ToList();
                }
            }
            catch (Exception ex)
            {
                // If an exception is thrown, log the event.
                _logger.LogError(ex, "Problem in ViewAllOrdersAsync action method " +
                    "in CMS controller.");
                // Redirect to error page.
                return RedirectToAction("Error", "Home");
            }
            return View(list);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> ViewOrderAsync(string orderId)
        {
            List<OrderLine> lineList = null;
            Order order = null;
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    order = context.order.Include(m => m.delivery).Where(m => m.orderId.ToString() == orderId).FirstOrDefault();
                    var orderLineList = await context.orderLine.Include(m => m.product).Include(m => m.order).ToListAsync();
                    lineList = orderLineList.Where(m => m.order.orderId.ToString() == orderId).ToList();
                }
            }
            catch (Exception ex)
            {
                // If an exception is thrown, log the event.
                _logger.LogError(ex, "Problem in ViewOrderAsync action method " +
                    "in CMS controller.");
                // Redirect to error page.
                return RedirectToAction("Error", "Home");
            }
            return View(new OrderView { orderLineList = lineList, order = order });
        }

        //============ update order and delivery=================
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrderAsync(string orderId, OrderView orderView)
        {
            Order order = null;
            Delivery delivery = null;
          
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    order = context.order.Include(m => m.delivery).Where(m => m.orderId.ToString() == orderId).FirstOrDefault();
                    order.orderStatus = orderView.orderStatus;
                    delivery = context.delivery.Where(m => m.deliveryId.ToString() == order.delivery.deliveryId.ToString()).FirstOrDefault();
                    delivery.deliverystatus = orderView.deliveryStatus;
                    context.order.Update(order);
                    context.delivery.Update(delivery);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // If an exception is thrown, log the event.
                _logger.LogError(ex, "Problem in UpdateOrderAsync action method " +
                    "in CMS controller.");
                // Redirect to error page.
                return RedirectToAction("ErrorOrderSummary", "CMS");
            }
            return RedirectToAction("ViewAllOrders");
        }

        // Request a PDF report of order information. HTTPGET==================
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult RequestOrderSummary()
        {
            decimal totalPrice;
            DateTime startDate; 
            DateTime endDate;
            string userId;

            try
            {

            }
            catch (Exception ex)
            {
                // If an exception is thrown, log the event.
                _logger.LogError(ex, "Problem in RequestOrderSummary action method " +
                    "in CMS controller.");
                // Redirect to the specific error page.
                return RedirectToAction("ErrorOrderSummary", "CMS");
            }

            return View();
        }

        // Generate a PDF report from order information. HTTPGET===============
        // Code for this technique is based on an article by Scott Hanselman
        // Reference: https://blog.elmah.io/generate-a-pdf-from-asp-net-core-for-free/
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> OrderSummaryPdf(decimal ?totalPrice, DateTime ?startDate, DateTime? endDate, string? userId)
        {
            List<Order> list;
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    list = await context.order.Include(m => m.delivery).ToListAsync();
                    list = list.OrderByDescending(m => m.paymentDate).ToList();
                }
            }
            catch (Exception ex)
            {
                // If an exception is thrown, log the event.
                _logger.LogError(ex, "Problem in OrderSummaryPDF action method " +
                    "in CMS controller.");
                // Redirect to the specific error page.
                return RedirectToAction("Error", "Home");
            }

            return await _generatePdf.GetPdf("Views/CMS/OrderSummaryPDF.cshtml", list);
        }
    }
}
