using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPInfrastructure.Services;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore;
using SDPWeb.ViewModels;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using System.Linq;
using SDPCore.Models.AccountModel;
using Newtonsoft.Json;

namespace SDP.Controllers
{
    public class CartController : Controller
    {
        ICustomer customer = null;
        private readonly IDbRepo _dbRepo;
        private readonly IPromotionService _promotionService;

        //=================== Constructor =======================
        public CartController(IDbRepo dbRepo, IPromotionService promotionService)
        {
            _dbRepo = dbRepo;
            _promotionService = promotionService;
        }

        //=================== Displays the cart page =======================
        public IActionResult Index(string Msg)
        {
            ViewData["Error"] = Msg;

            // Check for a valid ID in the session data, or create a new one if it doesn't exist.
            if (HttpContext.Session.GetString("Id") == null)
            {
                // Create a Guest Customer
                GuestCustomer guest = new GuestCustomer(_dbRepo);
                GlobalVar.customerList.Add(guest);

                // Store Guest Customer ID in Session Data
                string Id = guest.Id.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            customer = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"));
            return View(new CartView { cart = customer.cart, productList = _dbRepo.GetProductList().ToList() });
        }
        //===================Action for checking validity of promocode=======================
        [HttpPost]
        public IActionResult promoCodeCheck(string promoCode)
        {
            if (string.IsNullOrEmpty(promoCode))
            {
                return RedirectToAction("Index", new { Msg = "Invalid promo code" });
            }
            string appliedId = _promotionService.GetPromoProductId(promoCode);
            var customer = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"));
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            try
            {
                //checks if theres already a active code for the cart
                if (customer.cart.promotionActive)
                {
                    ViewData["Error"] = "Only one promotion allowed per order";
                    return View("Index", new CartView { cart = customer.cart, productList = _dbRepo.GetProductList().ToList() });
                }

                var productAppliedTo = _dbRepo.getProduct(appliedId);
                //validating promocode info  
                if (customer != null)
                {
                    if (_promotionService.validatePromo(promoCode))
                    {
                        if (customer.cart.cartList.ContainsKey(productAppliedTo.productId.ToString()))
                        {

                            customer.cart.cartList[productAppliedTo.productId.ToString()].discount = _dbRepo.getPromotionBypromoCode(promoCode).discount;
                            customer.cart.promotionActive = true;
                        }
                        else
                        {
                            ViewData["Error"] = "Promocode does not apply to any of the product in your cart.";
                            return View("Index", new CartView { cart = customer.cart, productList = _dbRepo.GetProductList().ToList() });
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "Promotion is Expired";
                        return View("Index", new CartView { cart = customer.cart, productList = _dbRepo.GetProductList().ToList() });
                    }
                }
                ViewData["Msg"] = "Promotion added!";
                return View("Index", new CartView { cart = customer.cart, productList = _dbRepo.GetProductList().ToList() });
            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        //===================Remove from cart=======================
        public IActionResult RemoveFromCart(string Id)
        {
            try
            {
                customer = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"));


                if (HttpContext.Session.GetString("count") != null)
                {
                    var count = int.Parse(HttpContext.Session.GetString("count"));
                    HttpContext.Session.SetString("count", (count - customer.cart.cartList[Id].quantity).ToString());
                }
                customer.cart.removeProductToCart(Id);
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }
       
         [HttpPost]
        public IActionResult Checkout(string Json, string stripeToken)
        {
            return RedirectToAction("Payment","Payment", new { checkoutViewJson = Json, stripeToken = stripeToken });
        }

        [HttpGet]
        public IActionResult Checkout(string checkoutViewJson)
        {
            ViewData["Json"] = checkoutViewJson;
            CheckoutView checkoutView = JsonConvert.DeserializeObject<CheckoutView>(checkoutViewJson);
            return View(checkoutView);
        }
        [HttpGet]
        public IActionResult DeliveryForm(decimal amount)
        {
            return View(new CheckoutView { amount = amount, delivery = new Delivery(), key = new Key() });
        }

        [HttpPost]
        public IActionResult DeliveryForm(CheckoutView checkoutView)
        {
            if (ModelState.IsValid)
            {
                checkoutView.key = new Key("pk_test_51JlMhVBxws1IZQJgbiaytkXHv77Q4SRWC3vSTaVsEZAbo2vSCOSLzdgbXx6cULFTdfeEm4k72wsR8bTKEXED0vGb00bdtUZmao", "payment", checkoutView.amount, "LKR", HttpContext.Session.GetString("Email")==null? "" : HttpContext.Session.GetString("Email"));
                string checkoutViewJson = JsonConvert.SerializeObject(checkoutView);
                return RedirectToAction("Checkout", "Cart",new { checkoutViewJson = checkoutViewJson });
            }
            return RedirectToAction("DeliveryForm", "Cart");
        }
    }
}

































































































































































































