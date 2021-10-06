using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPInfrastructure.Services;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore;

namespace SDP.Controllers
{
    public class CartController : Controller
    {
        ICustomer customer = null;
        private readonly IDbRepo _dbRepo;
        private readonly IPromotionService _promotionService;

        public CartController(IDbRepo dbRepo, IPromotionService promotionService)
        {
            _dbRepo = dbRepo;
            _promotionService = promotionService;
        }
        //===================Displays the cart page=======================
        public IActionResult Index()
        {
            //checking for valid id in session data creates a new one if none exist
            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer(_dbRepo);
                GlobalVar.customerList.Add(guest); 
                string Id = guest.userId.ToString(); 
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            customer = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"));
            return View(customer.cart);
        }
        //===================Action for checking validity of promocode=======================
        [HttpPost]
        public IActionResult promoCodeCheck(string promoCode) 
        {
            string appliedId = _promotionService.validatePromoCode(promoCode);
            var customer = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"));
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            try
            {
                if (customer.cart.promotionActive)
                {
                    ViewData["Error"] = "Only one promotion allowed per order";
                    return View("Index", customer.cart);
                }

                var productAppliedTo = _dbRepo.getProduct(appliedId);
                if (customer != null)
                {
                    if (_promotionService.validatePromoDate(promoCode))
                    {
                        if (customer.cart.cartList.ContainsKey(productAppliedTo.productId.ToString()))
                        {

                            customer.cart.cartList[productAppliedTo.productId.ToString()].discount = _dbRepo.getPromotion(promoCode).discount;
                            customer.cart.promotionActive = true;
                        }
                        else
                        {
                            ViewData["Error"] = "Promocode does not apply to any of the product in your cart.";
                            return View("Index", customer.cart);
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "Promotion is Expired";
                        return View("Index", customer.cart);
                    }
                }
                ViewData["Msg"] = "Promotion added!";
                return View("Index", customer.cart);
            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
            
        }
    }
}

































































































































































































