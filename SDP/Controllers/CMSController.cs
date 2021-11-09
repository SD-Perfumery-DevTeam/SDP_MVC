using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SDPWeb.ViewModels;

namespace SDP.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class CMSController : Controller
    {
        private UserManager<IdentityUser> _userManager { get; }
        private IEmailSender _emailSender { get; }
        private IDbRepo _dbRepo;
        private readonly IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> _contextFactory;

        public CMSController(UserManager<IdentityUser> userManager, IEmailSender emailSender, IDbRepo dbRepo, IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> contextFactory)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _dbRepo = dbRepo;
            _contextFactory = contextFactory;
        }


        public IActionResult Index(string Msg)
        {
            ViewData["Msg"] = Msg;
            return View();
        }
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
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "CMS", new { Msg = "Promotions has been sent" });
        }

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
                return RedirectToAction("Error", "Home");
            }
            return View(list);
        }

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
                return RedirectToAction("Error", "Home");
            }
            return View(new OrderView { orderLineList = lineList, order = order });
        }

        //============ update order and delivery=================
        [HttpPost]
        public async Task<IActionResult> UpdateOrderAsync(string orderId, OrderView orderView)
        {
            try
            {
                Order order = null;
                Delivery delivery = null;
                using (var context = _contextFactory.CreateDbContext())
                {
                    order = context.order.Where(m => m.orderId.ToString() == orderId).FirstOrDefault();
                    order.orderStatus = orderView.orderStatus;
                    context.order.Update(order);
                    await context.SaveChangesAsync();
                }
                using (var context = _contextFactory.CreateDbContext())
                {
                    delivery = context.delivery.Where(m => m.deliveryId.ToString() == orderView.order.delivery.deliveryId.ToString()).FirstOrDefault();
                    delivery.deliverystatus= orderView.deliveryStatus;
                    context.delivery.Update(delivery);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ViewAllOrders");
        }
    }
}
